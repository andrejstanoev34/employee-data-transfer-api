namespace EmployeeDataImporterApi.Domain.Dto;

public class StatusChangeDto
{
    public string EmployeeNumber  { get; set; } 
    public string Before { get; set; } 
    public string After  { get; set; }
}