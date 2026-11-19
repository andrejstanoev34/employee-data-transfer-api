namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportedEmployee
{
    public int RowNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? DateOfBirth { get; set; }
    public string? HireDate { get; set; }
    public string? DepartmentCode { get; set; }
    public string? RoleTitle { get; set; }
    public string? OfficeCity { get; set; }
    public string? Salary { get; set; }
    public string? Phone { get; set; }
    public string? ManagerEmployeeNumber { get; set; }
    public string? Notes { get; set; }
}