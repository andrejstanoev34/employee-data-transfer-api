namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportedSkill
{
    public int RowNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DateOfBirth { get; set; }
    public string? SkillName { get; set; }
}