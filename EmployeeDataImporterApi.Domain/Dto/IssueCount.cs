using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Dto;

public class IssueCount
{
    public String IssueType { get; set; }
    public int Count { get; set; }
}