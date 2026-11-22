using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;

namespace EmployeeDataImporterApi.Services.Implementation;

public class SkillValidator
{
    public List<ImportRowResult> ValidateSkills(ImportedSkillsData importedSkillsData, ReferenceData refData)
    {
        var skillRows = new List<ImportRowResult>();
        foreach (var skillRow in importedSkillsData.ImportedSkills)
        {
            var res = ValidateSkill(skillRow, refData);
            skillRows.Add(res);
        }
        
        return skillRows;
    }
    
    private ImportRowResult ValidateSkill(ImportedSkill skillRow, ReferenceData refData)
    {
        var rowResult = new ImportRowResult() { RowNumber = skillRow.RowNumber, FileType = FileType.Skill };
        var rowIssue = new RowIssues(rowResult);
        
        string? employeeNumber = Util.Clean(skillRow.EmployeeNumber);
        string? firstname = Util.Clean(skillRow.FirstName);
        string? lastname = Util.Clean(skillRow.LastName);
        string? dateOfBirth = Util.Clean(skillRow.DateOfBirth);
        string? skillName = Util.Clean(skillRow.SkillName);

        if (skillName == null)
        {
            rowIssue.Required("SkillName", ErrorType.MissingSkillNameField,"The skill name field is empty","");
            return rowResult;
        }

        var res = refData.SkillsByName.TryGetValue(skillName, out Skill? skill);

        if (!res)
        {
            rowIssue.Required("SkillName", ErrorType.InvalidSkillName, $"{skillName} does not exist", skillName);
            return rowResult;
        }
        
        if (employeeNumber != null)
        {
            if (!refData.EmployeeByNumber.TryGetValue(employeeNumber, out Employee? e))
            {
                // ne postoe employee so toj broj
                rowIssue.Required("EmployeeNumber", ErrorType.InvalidEmployeeNumber, $"{employeeNumber} does not exist", employeeNumber);
                return rowResult;
            }
            e.Skills.Add(new EmployeeSkill()
            {
                Employee = e, Skill = skill
            });
            rowResult.EmployeeNumber = employeeNumber;
            rowResult.Status = ImportStatus.Imported;
            return rowResult;
        }
        else
        {
            if (firstname == null || lastname == null || dateOfBirth == null)
            {
                rowIssue.Required("FirstName or LastName or DateOfBirth", ErrorType.MissingRequiredField, "Some of the three fields are missing", "");
                return rowResult;
            }
            else
            {
                DateOnly? dob = Util.ParseDate(dateOfBirth);
                string key = $"{firstname.ToLower()}|{lastname.ToLower()}|{dob!.Value:yyyy-MM-dd}";
                if (!refData.EmployeeByNameSurnameDob.TryGetValue(
                        key,
                        out List<Employee>? e))
                {
                    rowIssue.Required("FirstName, LastName, DateOfBirth", ErrorType.InvalidEmployeeNumber, $"Employee with credentials {firstname}, {lastname}, and {dateOfBirth} does not exist", "");
                    return rowResult;
                }

                var emp = e.FirstOrDefault();
                emp.Skills.Add(new EmployeeSkill()
                {
                    Employee = emp, Skill = skill
                });
                rowResult.EmployeeNumber = "";
                rowResult.Status = ImportStatus.Imported;
                return rowResult;
            }
        }
        
    }
}