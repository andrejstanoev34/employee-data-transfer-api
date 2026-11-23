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
    private readonly Repository<Organisation> _organisationRepository;

    public ImportService(EmployeeFileReaderFactory fileReaderFactory, EmployeeDataValidator employeeDataValidator, Repository<Employee> employeeRepository, Repository<EmployeeSkill> employeeSkillRepository, Repository<ImportRun> importRunRepository, Repository<Organisation> organisationRepository)
    {
        _fileReaderFactory = fileReaderFactory;
        _employeeDataValidator = employeeDataValidator;
        _employeeRepository = employeeRepository;
        _employeeSkillRepository = employeeSkillRepository;
        _importRunRepository = importRunRepository;
        _organisationRepository = organisationRepository;
    }

    public async Task<ImportRunDto> ImportData(List<IFormFile> files, Guid organisationId)
    {
        //read
        //moze samo imeto da go zejme so select???
        var organisation = await _organisationRepository.GetAsync(
            selector:x=>x,
            predicate:x=>x.Id==organisationId);
        var importRun = new ImportRun()
        {
            StartTime = DateTime.UtcNow,
            OrganisationId = organisationId,
            Organisation = organisation,
        };
        var extension = Path.GetExtension(files.First().FileName);
        var employeeFileReader = _fileReaderFactory.GetFileReader(extension);
        var fileNames = "";
        var importedData = new ImportedData();
        foreach (var formFile in files)
        {
            var fileName = formFile.FileName;
            fileNames += fileName;
            employeeFileReader.ReadFile(formFile.OpenReadStream(), importedData, fileName);
        }
        //validate
        var validationOutcome = await _employeeDataValidator.ValidateAsync(importedData, organisationId);
        
        //put in db
        var newEmployees = validationOutcome.EmployeeRows
            .Where(r => r.Employee != null)
            .Select(r => r.Employee!)
            .ToList();
        
        await _employeeRepository.InsertManyAsync(newEmployees);

        var newSkillEmployee = validationOutcome.SkillRows
            .Where(r => r.EmployeeSkill != null)
            .Select(r => r.EmployeeSkill!)
            .ToList();

        await _employeeSkillRepository.InsertManyAsync(newSkillEmployee);
        
        //return Import run 
        importRun.EndTime = DateTime.UtcNow;
        importRun.Status = ImportStatus.Imported;
        importRun.TotalRows = validationOutcome.Results.Count();
        importRun.ProcessedRows = newEmployees.Count + newSkillEmployee.Count;
        importRun.ImportRowResults = validationOutcome.Results.ToList();
        importRun.FileName = fileNames;
        importRun.Message = string.Join(", ", importedData.Message);
        var res = await _importRunRepository.InsertAsync(importRun);
        
        return res.ToImportRunDto();
    }

    public async Task<List<ImportRunDto>> GetAllImportRuns()
    {
        var res = (await _importRunRepository.GetAllAsync(
            selector: x => x,
            include: x=>x.Include(o=>o.Organisation)
        ))
        .ToList();

        return res.ToImportRunDtoList();
    }

    public async Task<ImportRunDto> GetImportRunById(Guid importRunId)
    {
        var res = (await _importRunRepository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == importRunId,
            include: x => x.Include(i => i.Organisation)
        ));
        return res.ToImportRunDto();
    }
    
}