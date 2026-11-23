using System.Globalization;
using System.Text.RegularExpressions;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;
using EmployeeDataImporterApi.Repository;
using EmployeeDataImporterApi.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDataImporterApi.Services.Implementation;

public class EmployeeDataValidator
{
    private readonly Repository<Employee> _employeeRepository;
    private readonly Repository<Role> _roleRepository;
    private readonly Repository<Department> _departmentRepository;
    private readonly Repository<Office> _officeRepository;
    private readonly Repository<Skill> _skillRepository;
    private readonly Repository<EmployeeSkill> _employeeSkillRepository;
    private readonly EmployeeValidator _employeeValidator;
    private readonly SkillValidator _skillValidator;
    
    
    public EmployeeDataValidator(Repository<Employee> employeeRepository, Repository<Role> roleRepository, Repository<Department> departmentRepository, Repository<Office> officeRepository, Repository<Skill> skillRepository, EmployeeValidator employeeValidator, SkillValidator skillValidator, Repository<EmployeeSkill> employeeSkillRepository)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _departmentRepository = departmentRepository;
        _officeRepository = officeRepository;
        _skillRepository = skillRepository;
        _employeeValidator = employeeValidator;
        _skillValidator = skillValidator;
        _employeeSkillRepository = employeeSkillRepository;
    }
    
    public async Task<ValidationOutcome> ValidateAsync(ImportedData importedData, Guid organisationId)
    {
        var refData = await LoadReferenceDataAsync(organisationId);
        var rows = _employeeValidator.ValidateEmployees(importedData.ImportedEmployeesData, refData);
        
        var res = rows
            .Where(r => r.Employee != null)
            .Select(r => r.Employee);
        
        var skippedEmployees = rows.Where(r => r.Employee == null).ToList();

        foreach (var e in res)
        {
            refData.EmployeeByNumber.Add(e!.EmployeeNumber,e);
            if (e.DateOfBirth != null)
            {
                string key = $"{e.FirstName}|{e.LastName}|{e.DateOfBirth.Value:yyyy-MM-dd}";

                if (!refData.EmployeeByNameSurnameDob.TryGetValue(key, out List<Employee>? employeeList))
                {
                    employeeList = new List<Employee>();
                    refData.EmployeeByNameSurnameDob[key] = employeeList;
                }
                employeeList.Add(e);
            }
        }
        
        var skillsRows = _skillValidator.ValidateSkills(importedData.ImportedSkillsData, refData);
        
        // proverka dali employeeNumber - SkillId vekje postoe
        var skillRes = skillsRows.Where(r => r.EmployeeSkill != null).ToList();
        var skipped = skillsRows.Where(r => r.EmployeeSkill == null).ToList();
        foreach (var r in skillRes)
        {
            EmployeeSkill employeeSkill = r.EmployeeSkill;
            if (refData.EmployeeSkills.ContainsKey((employeeSkill.Employee.EmployeeNumber, employeeSkill.SkillId)))
            {
                r.Result.Status = ImportStatus.Skipped;
                r.Result.Errors.Add( new ImportRowError()
                {
                    Field = "",
                    Message = $"The combination {employeeSkill.Employee.EmployeeNumber} | {employeeSkill.SkillId} already exists.",
                    Value = "",
                    ErrorType = ErrorType.DuplicateSkillEmployeeNumber
                } );
                // i treba EmployeeSkill da go stajme na null
                r.EmployeeSkill = null;
            }
            else
            {
                refData.EmployeeSkills.Add((employeeSkill.Employee.EmployeeNumber, employeeSkill.SkillId), employeeSkill);
            }
        }
        
        return new ValidationOutcome { EmployeeRows = rows, SkillRows = skillsRows };
    }
    
    
    private async Task<ReferenceData> LoadReferenceDataAsync(Guid organisationId)
    {
        var roles = await _roleRepository.GetAllAsync(selector: r => new { r.Title, r.Id });
        var departments = await _departmentRepository.GetAllAsync(
            selector: d => new { d.Code, d.Id }, predicate: d => d.OrganisationId == organisationId);
        var offices = await _officeRepository.GetAllAsync(
            selector: o => new { o.City, o.Id }, predicate: o => o.OrganisationId == organisationId);
        var numbers = await _employeeRepository.GetAllAsync(
            selector: e => e.EmployeeNumber, predicate: e => e.OrganisationId == organisationId);
        var emails = await _employeeRepository.GetAllAsync(selector: e => e.Email);
        var skills = await _skillRepository.GetAllAsync(
            selector: x => x
        );
        var employees = await _employeeRepository.GetAllAsync(
            selector:x=>x, predicate:x=>x.OrganisationId == organisationId);
        var employeeSkills = await _employeeSkillRepository.GetAllAsync(
            selector: x => x,
            predicate: x => x.Employee.OrganisationId == organisationId,
            include: x => x.Include(e => e.Employee)
        );

        return new ReferenceData
        {
            OrganisationId = organisationId,
            RolesByTitle = roles.ToDictionary(r => Util.NormalizeTitle(r.Title), r => r.Id, StringComparer.OrdinalIgnoreCase),
            DepartmentsByCode = departments.ToDictionary(d => d.Code.Trim(), d => d.Id, StringComparer.OrdinalIgnoreCase),
            OfficesByCity = offices.ToDictionary(o => o.City.Trim(), o => o.Id, StringComparer.OrdinalIgnoreCase),
            SkillsByName = skills.ToDictionary(s=>s.Name.Trim(), s=>s, StringComparer.OrdinalIgnoreCase),
            EmployeeByNumber = employees.ToDictionary(e=>e.EmployeeNumber,e=>e, StringComparer.OrdinalIgnoreCase),
            EmployeeByNameSurnameDob = employees
                .Where(e => e.DateOfBirth != null)
                .GroupBy(
                    e => $"{e.FirstName}|{e.LastName}|{e.DateOfBirth!.Value:yyyy-MM-dd}",
                    StringComparer.OrdinalIgnoreCase
                )
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList(),
                    StringComparer.OrdinalIgnoreCase
                ),
            TakenEmployeeNumbers = numbers.ToHashSet(StringComparer.OrdinalIgnoreCase),
            TakenEmails = emails.ToHashSet(StringComparer.OrdinalIgnoreCase),
            EmployeeSkills = employeeSkills.ToDictionary(es=>(es.Employee.EmployeeNumber, es.SkillId), es=>es)
        };
    }
}