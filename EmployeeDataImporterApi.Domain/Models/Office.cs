using EmployeeDataImporterApi.Domain.Common;

namespace EmployeeDataImporterApi.Domain.Models;

public class Office: BaseEntity
{
    public string City { get; set; }
    public string Country { get; set; }
    
    public Guid OrganisationId { get; set; }
    public Organisation Organisation { get; set; }
    
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}