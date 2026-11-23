namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportRunDto
{
    public Guid ImportId {get; set;}
    public string FileName {get; set;}
    public string OrganisationName {get; set;}
    public string Status {get; set;}
    public string Message {get; set;}
    public int TotalRows {get; set;}
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int ProcessedRows {get; set;}
}