using EmployeeDataImporterApi.Domain.Common;

namespace EmployeeDataImporterApi.Domain.Models;

public class Office: BaseEntity
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    
    public Guid OrganisationId { get; set; }
    public Organisation? Organisation { get; set; }
    
    public ICollection<DepartmentOffice> Departments { get; set; } = new List<DepartmentOffice>();
    
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}