namespace EmployeeDataImporterApi.Domain.Dto;

public class RunDifferenceDto
{
    public List<string> NewEmployeeNumbers { get; set; } = new List<string>();
    public List<string> MissingEmployeeNumbers { get; set; } = new List<string>();
    public List<StatusChangeDto> Changed { get; set; } = new List<StatusChangeDto>();
}