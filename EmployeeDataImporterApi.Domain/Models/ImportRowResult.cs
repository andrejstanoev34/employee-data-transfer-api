using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Models;

public class ImportRowResult: BaseEntity
{
    public int RowNumber { get; set; }
    public string EmployeeNumber { get; set; }
    public ImportStatus Status { get; set; }
    
    public Guid ImportRunId { get; set; }
    public ImportRun? ImportRun { get; set; }
    
    public ICollection<ImportRowError> Errors { get; set; } = new List<ImportRowError>();
}