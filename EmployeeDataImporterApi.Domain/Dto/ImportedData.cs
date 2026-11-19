namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportedData
{
    public ImportedEmployeesData ImportedEmployeesData { get; set; } = new ImportedEmployeesData();
    public ImportedSkillsData ImportedSkillsData { get; set; } =  new ImportedSkillsData();
}