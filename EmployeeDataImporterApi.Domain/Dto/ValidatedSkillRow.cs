using EmployeeDataImporterApi.Domain.Models;

namespace EmployeeDataImporterApi.Domain.Dto;

public class ValidatedSkillRow
{
    public required ImportRowResult Result { get; set; }
    public EmployeeSkill? EmployeeSkill { get; set; }
}