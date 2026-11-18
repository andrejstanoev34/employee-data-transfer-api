using EmployeeDataImporterApi.Domain.Common;

namespace EmployeeDataImporterApi.Domain.Models;

public class Organisation: BaseEntity
{
    public string Name { get; set; }
    
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    
    public ICollection<Office> Offices { get; set; } = new List<Office>();
    
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    
    public ICollection<ImportRun> ImportRuns { get; set; } = new List<ImportRun>();
}