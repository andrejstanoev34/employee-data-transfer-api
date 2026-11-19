namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportedEmployeesData
{
    public List<ImportedEmployee> ImportedEmployees {get; set;} = new List<ImportedEmployee>();
    public int TotalEmployeeRows { get; set; } = 0;
    public string Message {get; set;} = "";
}