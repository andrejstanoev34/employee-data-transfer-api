using EmployeeDataImporterApi.Domain.Common;

namespace EmployeeDataImporterApi.Domain.Models;

public class Skill: BaseEntity
{
    public string Name { get; set; }
    
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}