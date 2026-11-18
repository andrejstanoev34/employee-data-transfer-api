using EmployeeDataImporterApi.Domain.Common;

namespace EmployeeDataImporterApi.Domain.Models;

public class EmployeeSkill: BaseEntity
{
    public Guid SkillId {get; set;}
    public Guid EmployeeId {get; set;}
    public Skill? Skill {get; set;}
    public Employee? Employee {get; set;}
}