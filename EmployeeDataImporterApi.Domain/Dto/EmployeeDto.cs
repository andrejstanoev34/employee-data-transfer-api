namespace EmployeeDataImporterApi.Domain.Dto;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string EmployeeNumber { get; set; }
    public string DepartmentName { get; set; }
    public string OrganisationName { get; set; }
}