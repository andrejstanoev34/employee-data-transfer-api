namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportedEmployee
{
    public int RowNumber { get; set; }
    public string? EmployeeNumber { get; set; } // required
    public string? FirstName { get; set; } // required
    public string? LastName { get; set; } // required
    public string? Email { get; set; } // required
    public string? DateOfBirth { get; set; }
    public string? HireDate { get; set; } // required
    public string? DepartmentCode { get; set; } // required
    public string? RoleTitle { get; set; } // required
    public string? OfficeCity { get; set; }
    public string? Salary { get; set; }
    public string? Phone { get; set; }
    public string? ManagerEmployeeNumber { get; set; }
    public string? Notes { get; set; }
}