using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Models;

public class ImportRun: BaseEntity
{
    public string FileName {get; set;} = string.Empty;
    
    public Guid OrganisationId {get; set;}
    public Organisation? Organisation {get; set;}
    
    public DateTime StartTime {get; set;}
    public DateTime? EndTime {get; set;}
    
    public ImportStatus RunStatus {get; set;}
    public string Message {get; set;} = string.Empty;
    public int TotalRows {get; set;}
    public int ProcessedRows {get; set;}
    
    public ICollection<ImportRowResult> ImportRowResults {get; set;} = new List<ImportRowResult>();
}