using EmployeeDataImporterApi.Domain;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;
using EmployeeDataImporterApi.Repository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDataImporterApi.Services.Implementation;

public class StatisticsQueries
{
    private readonly EmployeeDataImporterContext _context;

    public StatisticsQueries(EmployeeDataImporterContext context)
    {
        _context = context;
    }

    public async Task<List<DepartmentTopEarnersDto>> TopEarners()
    {

        var departments = _context.Departments
            .Include(d => d.Employees)
            .Select(d => new DepartmentTopEarnersDto()
            {
                DepartmentId = d.Id,
                DepartmentName = d.Name,
                DepartmentCode = d.Code,
                TopEarnerDtos = d.Employees
                    .Where(e => e.Salary != null)
                    .OrderByDescending(e => e.Salary)
                    .ThenBy(e => e.HireDate)
                    .Take(3)
                    .Select(e => new TopEarnerDto()
                    {
                        EmployeeNumber = e.EmployeeNumber,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Salary = e.Salary!.Value,
                    })
            }).ToList();
        
        // var queryString = departments.AsQueryable().ToQueryString();
        return departments;
    }


    public async Task<List<DepartmentEmployeeCount>> EmptySpots()
    {
        var departmentEmployeeCounts = _context.Departments
            .Include(d => d.Employees)
            .Include(d => d.Offices).ThenInclude(d => d.Office).ThenInclude(o => o.Employees)
            .Select(d => new DepartmentEmployeeCount()
            {
                DepartmentId = d.Id,
                DepartmentName = d.Name,
                NumberOfEmployees = d.Employees.Count,
                OfficeEmployeeCountDtos = d.Offices.Select(o => o.Office)
                    .Select(o => new OfficeEmployeeCountDto()
                    {
                        OfficeId = o.Id,
                        OfficeCity = o.City,
                        OfficeCountry = o.Country,
                        NumberOfEmployees = o.Employees.Count
                    }).AsEnumerable().ToList()
            }).ToList();
        
        // var nes = res.ToQueryString();
        
        return departmentEmployeeCounts;
    }

    public async Task<List<EmployeeDto>> EmployeesWithSkills(List<Guid> skills)
    {
        var employeesWithSkills = _context.Employees
            .Include(e => e.Organisation)
            .Include(e => e.Department)
            .Where(e=> skills.All(id=>e.Skills.Any(s=>s.SkillId==id)))
            .Select(e => new EmployeeDto()
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                EmployeeNumber = e.EmployeeNumber,
                DepartmentName = e.Department.Name,
                OrganisationName =  e.Organisation.Name,
            }).ToList();
        
        
        return employeesWithSkills;
    }

    public async Task<List<TenureCohortsDto>> TenureCohorts()
    {

        var countPerDepartmentPerYear = _context.Employees
            .GroupBy(e => new { e.DepartmentId, DepartmentName = e.Department.Name, e.HireDate.Year })
            .Select(g=>new
            {
                g.Key.DepartmentId,
                g.Key.DepartmentName,
                g.Key.Year,
                HireCount = g.Count()
            })
            .OrderBy(x=>x.DepartmentId).ThenBy(x=>x.Year)
            .ToList();
        
        int runningTotal = 0;
        
        Guid nowDepId = countPerDepartmentPerYear.First().DepartmentId;
        
        List<TenureCohortsDto> tenureCohortsDto = new List<TenureCohortsDto>();
        
        foreach (var o in countPerDepartmentPerYear)
        {
            if (o.DepartmentId == nowDepId)
            {
                runningTotal += o.HireCount;
                tenureCohortsDto.Add(TenureCohortsDto.Create(o.DepartmentId, o.DepartmentName, o.Year, o.HireCount, runningTotal));
            }
            else
            {
                nowDepId = o.DepartmentId;
                runningTotal = o.HireCount;
            }
        }
        return tenureCohortsDto;
    }

    public async Task<RunDifferenceDto> RunDiff(Guid organisationId, Guid runOneId, Guid runTwoId)
    {
        var found = await _context.ImportRuns.CountAsync(r =>
            r.OrganisationId == organisationId && (r.Id == runOneId || r.Id == runTwoId));
        
        if (runOneId == runTwoId || found != 2)
            throw new KeyNotFoundException("One or both import runs were not found for this organisation.");

        var first = StatusPerEmployee(runOneId); 
        
        var second = StatusPerEmployee(runTwoId);

        var newQuery = second
            .Where(b => !first.Any(a => a.EmployeeNumber == b.EmployeeNumber))
            .Select(b => b.EmployeeNumber)
            .OrderBy(n => n);

        var missingQuery = first
            .Where(a => !second.Any(b => b.EmployeeNumber == a.EmployeeNumber))
            .Select(a => a.EmployeeNumber)
            .OrderBy(n => n);

        var changedQuery = first
            .Join(
                second,
                a => a.EmployeeNumber,
                b => b.EmployeeNumber,
                (a, b) => new { a.EmployeeNumber, Before = a.Status, After = b.Status })
            .Where(x => x.Before != x.After)
            .OrderBy(x => x.EmployeeNumber);

        var changed = await changedQuery.ToListAsync();
        
        return new RunDifferenceDto()
        {
            NewEmployeeNumbers = await newQuery.ToListAsync(),
            MissingEmployeeNumbers = await missingQuery.ToListAsync(),
            Changed = changed.Select(c => new StatusChangeDto()
            {
                EmployeeNumber = c.EmployeeNumber,
                Before = c.Before.ToString(),
                After = c.After.ToString()
            }).ToList()
        };
    }
    
    private IQueryable<EmployeeNumberStatus> StatusPerEmployee(Guid runId) =>
        _context.Set<ImportRowResult>()
            .Where(r => r.ImportRunId == runId
                        && r.FileType == FileType.Employee
                        && r.EmployeeNumber != null)
            .GroupBy(r => r.EmployeeNumber!)
            .Select(g => new EmployeeNumberStatus(
                g.Key,
                g.OrderBy(r => r.RowNumber).Select(r => r.RowStatus).First()));
    
}
