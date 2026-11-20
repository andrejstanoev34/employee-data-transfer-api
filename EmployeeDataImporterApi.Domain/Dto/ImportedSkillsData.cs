namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportedSkillsData
{
    public List<ImportedSkill> ImportedSkills { get; set; } = new List<ImportedSkill>();
    public int TotalSkillRows { get; set; } = 0;
    public bool HasIssue => Message.Count != 0;
    public List<string> Message { get; set; } = new List<string>();
}