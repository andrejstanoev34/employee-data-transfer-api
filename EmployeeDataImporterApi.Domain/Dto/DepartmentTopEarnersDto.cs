namespace EmployeeDataImporterApi.Domain.Dto;

public class DepartmentTopEarnersDto
{
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string DepartmentCode { get; set; } = string.Empty;
    public List<TopEarnerDto> TopEarnerDtos { get; set; } = new List<TopEarnerDto>();
}