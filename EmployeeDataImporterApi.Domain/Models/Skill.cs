using EmployeeDataImporterApi.Domain.Common;

namespace EmployeeDataImporterApi.Domain.Models;

public class Skill: BaseEntity
{
    public string Name { get; set; }
    
    public ICollection<EmployeeSkill> Employees { get; set; } = new List<EmployeeSkill>();
}