using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Dto;

public class ErrorCount
{
    public String ErrorType { get; set; }
    public int Count { get; set; }
}