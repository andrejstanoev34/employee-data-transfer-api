namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportedEmployeesData
{
    public List<ImportedEmployee> ImportedEmployees {get; set;} = new List<ImportedEmployee>();
    public int TotalEmployeeRows { get; set; } = 0;
    public bool HasIssue { get; set; } = false;
    public List<string> Message { get; set; } = new List<string>();
}