using EmployeeDataImporterApi.Domain.Models;

namespace EmployeeDataImporterApi.Domain.Dto;

public class ReferenceData
{
    public Guid OrganisationId { get; init; }
    public DateOnly Today { get; init; } = DateOnly.FromDateTime(DateTime.Today);
    public Dictionary<string, Guid> DepartmentsByCode { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, Guid> OfficesByCity { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, Guid> RolesByTitle { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, Skill> SkillsByName { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, Employee> EmployeeByNumber { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, List<Employee>> EmployeeByNameSurnameDob { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public HashSet<string> TakenEmployeeNumbers { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public HashSet<string> TakenEmails { get; init; } = new(StringComparer.OrdinalIgnoreCase);
}