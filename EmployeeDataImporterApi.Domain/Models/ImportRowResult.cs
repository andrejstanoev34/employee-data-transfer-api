using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Models;

public class ImportRowResult: BaseEntity
{
    public int RowNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public ImportRowStatus RowStatus { get; set; }
    
    public Guid ImportRunId { get; set; }
    public ImportRun? ImportRun { get; set; }
    
    public FileType FileType { get; set; }
    
    public List<ImportRowIssue> Issues { get; set; } = new List<ImportRowIssue>();
}