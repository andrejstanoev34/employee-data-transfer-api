using System.Globalization;
using System.Text.RegularExpressions;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;
using EmployeeDataImporterApi.Repository;
using EmployeeDataImporterApi.Domain.Common;

namespace EmployeeDataImporterApi.Services.Implementation;

public class EmployeeDataValidator
{
    private readonly Repository<Employee> _employeeRepository;
    private readonly Repository<Role> _roleRepository;
    private readonly Repository<Department> _departmentRepository;
    private readonly Repository<Office> _officeRepository;
    private readonly Repository<Skill> _skillRepository;
    private readonly EmployeeValidator _employeeValidator;
    private readonly SkillValidator _skillValidator;
    
    public EmployeeDataValidator(Repository<Employee> employeeRepository, Repository<Role> roleRepository, Repository<Department> departmentRepository, Repository<Office> officeRepository, Repository<Skill> skillRepository, EmployeeValidator employeeValidator, SkillValidator skillValidator)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _departmentRepository = departmentRepository;
        _officeRepository = officeRepository;
        _skillRepository = skillRepository;
        _employeeValidator = employeeValidator;
        _skillValidator = skillValidator;
    }
    
    public async Task<ValidationOutcome> ValidateAsync(ImportedData importedData, Guid organisationId)
    {
        var refData = await LoadReferenceDataAsync(organisationId);
        var rows = _employeeValidator.ValidateEmployees(importedData.ImportedEmployeesData, refData);
        
        var res = rows
            .Where(r => r.Employee != null)
            .Select(r => r.Employee);

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

        return new ValidationOutcome { Rows = rows };
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
            TakenEmails = emails.ToHashSet(StringComparer.OrdinalIgnoreCase)
        };
    }
}