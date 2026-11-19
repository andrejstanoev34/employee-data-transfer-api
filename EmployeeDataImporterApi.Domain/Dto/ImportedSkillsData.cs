namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportedSkillsData
{
    public List<ImportedSkill> ImportedSkills { get; set; } = new List<ImportedSkill>();
    public int TotalSkillRows { get; set; } = 0;
    public string Message {get; set;} = "";
}