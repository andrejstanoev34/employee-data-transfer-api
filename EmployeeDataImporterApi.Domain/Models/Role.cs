using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Models;

public class Role: BaseEntity
{
    public string Title { get; set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}