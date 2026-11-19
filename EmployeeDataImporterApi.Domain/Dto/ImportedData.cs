namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportedData
{
    private readonly List<string> _messages = new();

    public ImportedEmployeesData ImportedEmployeesData { get; set; } = new();
    public ImportedSkillsData ImportedSkillsData { get; set; } = new();
    public void AddMessage(string message) => _messages.Add(message);
    public List<string> Message =>
        _messages
            .Concat(ImportedEmployeesData.Message)
            .Concat(ImportedSkillsData.Message)
            .ToList();

    public bool HasIssue => Message.Count > 0;
}