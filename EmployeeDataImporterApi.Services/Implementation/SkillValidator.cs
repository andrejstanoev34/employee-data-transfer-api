using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;

namespace EmployeeDataImporterApi.Services.Implementation;

public class SkillValidator
{
    public List<ValidatedSkillRow> ValidateSkills(ImportedSkillsData importedSkillsData, ReferenceData refData)
    {
        var skillRows = new List<ValidatedSkillRow>();
        foreach (var skillRow in importedSkillsData.ImportedSkills)
        {
            var res = ValidateSkill(skillRow, refData);
            skillRows.Add(res);
        }

        return skillRows;
    }

    private ValidatedSkillRow ValidateSkill(ImportedSkill skillRow, ReferenceData refData)
    {
        var rowResult = new ImportRowResult
        {
            RowNumber = skillRow.RowNumber,
            FileType = FileType.Skill,
            Status = ImportStatus.Skipped
        };
        var rowIssue = new RowIssues(rowResult);

        string? employeeNumber = Util.Clean(skillRow.EmployeeNumber);
        string? firstname = Util.Clean(skillRow.FirstName);
        string? lastname = Util.Clean(skillRow.LastName);
        string? dateOfBirth = Util.Clean(skillRow.DateOfBirth);
        string? skillName = Util.Clean(skillRow.SkillName);

        Skill? skill = null;
        Employee? employee = null;

        if (skillName == null)
        {
            rowIssue.Required("SkillName", ErrorType.MissingSkillNameField, "The skill name field is empty", "");
        }
        else if (!refData.SkillsByName.TryGetValue(skillName, out skill))
        {
            rowIssue.Required("SkillName", ErrorType.InvalidSkillName, $"{skillName} does not exist", skillName);
        }
        else if (employeeNumber != null)
        {
            if (!refData.EmployeeByNumber.TryGetValue(employeeNumber, out employee))
                rowIssue.Required("EmployeeNumber", ErrorType.InvalidEmployeeNumber, $"{employeeNumber} does not exist",
                    employeeNumber);
        }
        else if (firstname == null || lastname == null || dateOfBirth == null)
        {
            rowIssue.Required("FirstName, LastName, DateOfBirth", ErrorType.MissingRequiredField,
                "Some of the three fields are missing", "");
        }
        else
        {
            var dob = Util.ParseDate(dateOfBirth);

            if (dob == null)
            {
                rowIssue.Required("DateOfBirth", ErrorType.DateOfBirthInvalid, "Date of birth is not a valid date",
                    skillRow.DateOfBirth);
            }
            else
            {
                var key = $"{firstname.ToLower()}|{lastname.ToLower()}|{dob.Value:yyyy-MM-dd}";

                if (!refData.EmployeeByNameSurnameDob.TryGetValue(key, out var matches))
                    rowIssue.Required("FirstName, LastName, DateOfBirth", ErrorType.InvalidEmployeeNumber,
                        $"Employee with credentials {firstname}, {lastname}, and {dateOfBirth} does not exist", "");
                else if (matches.Count > 1)
                    rowIssue.Required("FirstName, LastName, DateOfBirth", ErrorType.SkillEmployeeAmbiguous,
                        $"More than one employee matches {firstname}, {lastname}, and {dateOfBirth}", "");
                else
                    employee = matches[0];
            }
        }

        EmployeeSkill? employeeSkill = null;

        if (employee != null && skill != null)
        {
            employeeSkill = new EmployeeSkill { Employee = employee, SkillId = skill.Id };
            rowResult.EmployeeNumber = employee.EmployeeNumber;
            rowResult.Status = ImportStatus.Imported;
        }

        return new ValidatedSkillRow { Result = rowResult, EmployeeSkill = employeeSkill };
    }
}