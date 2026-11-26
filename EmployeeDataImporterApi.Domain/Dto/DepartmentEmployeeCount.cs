namespace EmployeeDataImporterApi.Domain.Dto;

public class DepartmentEmployeeCount
{
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int NumberOfEmployees { get; set; } = 0;
    public ICollection<OfficeEmployeeCountDto> OfficeEmployeeCountDtos { get; set; } = new List<OfficeEmployeeCountDto>();
}