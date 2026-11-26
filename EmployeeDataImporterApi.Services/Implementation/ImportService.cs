using DocumentFormat.OpenXml.Office2010.ExcelAc;
using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;
using EmployeeDataImporterApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDataImporterApi.Services.Implementation;

public class ImportService
{
    private readonly EmployeeFileReaderFactory _fileReaderFactory;
    private readonly EmployeeDataValidator _employeeDataValidator;
    private readonly Repository<Employee> _employeeRepository;
    private readonly Repository<EmployeeSkill> _employeeSkillRepository;
    private readonly Repository<ImportRun> _importRunRepository;

    public ImportService(EmployeeFileReaderFactory fileReaderFactory, EmployeeDataValidator employeeDataValidator, Repository<Employee> employeeRepository, Repository<EmployeeSkill> employeeSkillRepository, Repository<ImportRun> importRunRepository, Repository<Organisation> organisationRepository)
    {
        _fileReaderFactory = fileReaderFactory;
        _employeeDataValidator = employeeDataValidator;
        _employeeRepository = employeeRepository;
        _employeeSkillRepository = employeeSkillRepository;
        _importRunRepository = importRunRepository;
    }

    public async Task<ImportRunDto> ImportData(List<IFormFile> files, Guid organisationId)
    {
        var importRun = new ImportRun()
        {
            StartTime = DateTime.UtcNow,
            OrganisationId = organisationId,
        };
        var fileNames = new List<string>();
        var importedData = new ImportedData();
        
        var extension = Path.GetExtension(files.First().FileName);
        
        var employeeFileReader = _fileReaderFactory.GetFileReader(extension);
        
        //read
        foreach (var formFile in files)
        {
            var fileName = formFile.FileName;
            fileNames.Add(fileName);
            await using var stream = formFile.OpenReadStream();
            employeeFileReader.ReadFile(stream, importedData, fileName);
        }
        
        //validate
        var validationOutcome = await _employeeDataValidator.ValidateAsync(importedData, organisationId);
        
        //put in db
        var newEmployees = validationOutcome.EmployeeRows
            .Where(r => r.Employee != null)
            .Select(r => r.Employee!)
            .ToList();
        var newSkillEmployee = validationOutcome.SkillRows
            .Where(r => r.EmployeeSkill != null)
            .Select(r => r.EmployeeSkill!)
            .ToList();
        
        await _employeeRepository.InsertManyAsync(newEmployees);
        await _employeeSkillRepository.InsertManyAsync(newSkillEmployee);
        
        //return Import run 
        importRun.EndTime = DateTime.UtcNow;
        importRun.RunStatus = ImportStatus.Complete;
        importRun.TotalRows = validationOutcome.Results.Count();
        importRun.ProcessedRows = newEmployees.Count + newSkillEmployee.Count;
        importRun.ImportRowResults = validationOutcome.Results.ToList();
        importRun.FileName = string.Join("_", fileNames);
        importRun.Message = string.Join(", ", importedData.Messages);
        
        var newImportRun = await _importRunRepository.InsertAsync(importRun);
        
        return newImportRun.ToImportRunDto();
    }

    public async Task<List<ImportRunDto>> GetAllImportRuns()
    {
        var importRuns = (await _importRunRepository.GetAllAsync(
            selector: x => x,
            include: x=>x.Include(o=>o.Organisation)
        ))
        .ToList();

        return importRuns.ToImportRunDtoList();
    }

    public async Task<ImportRunDto?> GetImportRunById(Guid importRunId)
    {
        var importRun = (await _importRunRepository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == importRunId,
            include: x => x.Include(i => i.Organisation)
        ));

        if (importRun == null)
        {
            throw new ArgumentException($"Import run with id {importRunId} not found");
        }
        
        return importRun.ToImportRunDto();
    }

    public async Task<ImportStatsDto> GetImportStats()
    {
        var importRuns = await _importRunRepository.GetAllAsync(
            selector: x => x,
            include: x => x.Include(i => i.Organisation).Include(i => i.ImportRowResults).ThenInclude(r => r.Issues)
        );
        
        var errorCount = importRuns
            .SelectMany(i=>i.ImportRowResults)
            .SelectMany(r=>r.Issues)
            .GroupBy(r=>r.IssueType)
            .OrderByDescending(g=>g.Count())
            .Select(g=>new ErrorCount()
            {
                ErrorType = g.Key.ToString(),
                Count = g.Count()
            })
            .ToList();

        var numSkippedAndPartial = importRuns
            .GroupBy(i => i.OrganisationId)
            .Select(g => new OrganisationSkippedPartialCount()
            {
                OrganisationId = g.Key,
                SkippedCount = g.SelectMany(r => r.ImportRowResults)
                    .Count(r => r.RowStatus == ImportRowStatus.Skipped),
                PartialCount = g.SelectMany(r => r.ImportRowResults)
                    .Count(r => r.RowStatus == ImportRowStatus.Partial)
            })
            .ToList();
            
        
        return new ImportStatsDto()
        {
            ErrorCounts = errorCount,
            OrganisationSkippedPartial = numSkippedAndPartial
        };
    }
    
}