using EmployeeDataImporterApi.Domain.Common;

namespace EmployeeDataImporterApi.Domain.Models;

public class Skill: BaseEntity
{
    public string Name { get; set; } = string.Empty ;
    
    public ICollection<EmployeeSkill> Employees { get; set; } = new List<EmployeeSkill>();
}