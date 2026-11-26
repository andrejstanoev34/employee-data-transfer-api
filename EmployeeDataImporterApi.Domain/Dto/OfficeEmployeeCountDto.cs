namespace EmployeeDataImporterApi.Domain.Dto;

public class OfficeEmployeeCountDto
{
    public Guid OfficeId { get; set; }
    public string OfficeCity { get; set; } = string.Empty;
    public string OfficeCountry { get; set; } = string.Empty;
    public int NumberOfEmployees { get; set; } = 0;
}