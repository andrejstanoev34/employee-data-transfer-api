using EmployeeDataImporterApi.Domain.Common;

namespace EmployeeDataImporterApi.Domain.Models;

public class Department: BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    public Guid OrganisationId { get; set; }
    public Organisation? Organisation { get; set; }
    
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<DepartmentOffice> Offices { get; set; } = new List<DepartmentOffice>();
}