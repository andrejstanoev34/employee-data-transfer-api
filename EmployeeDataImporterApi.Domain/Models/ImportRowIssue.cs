using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Models;

public class ImportRowIssue
{
    public Guid Id {get; set;}
    public string Field { get; set; } = string.Empty;
    public string Message {get; set;} = string.Empty;
    public string Value {get; set;} = string.Empty;
    public IssueType IssueType {get; set;}
    
    public Guid ImportRowResultId {get; set;}
    public ImportRowResult? ImportRowResult {get; set;}
}