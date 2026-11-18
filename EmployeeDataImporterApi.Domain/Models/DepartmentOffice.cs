using EmployeeDataImporterApi.Domain.Common;

namespace EmployeeDataImporterApi.Domain.Models;

public class DepartmentOffice : BaseEntity
{
    public Guid DepartmentId { get; set; }
    public Guid OfficeId { get; set; }
    public Department? Department { get; set; }
    public Office? Office { get; set; }
}