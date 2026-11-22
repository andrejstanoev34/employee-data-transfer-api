using EmployeeDataImporterApi.Domain.Models;

namespace EmployeeDataImporterApi.Domain.Dto;

public class ValidatedEmployeeRow
{
    public required ImportRowResult Result { get; init; }
    public Employee? Employee { get; set; }
    public string? ManagerEmployeeNumber { get; init; }
}
