using EmployeeDataImporterApi.Domain.Models;

namespace EmployeeDataImporterApi.Domain.Dto;

public class ValidationOutcome
{
    public required List<ValidatedEmployeeRow> EmployeeRows { get; init; }
    public required List<ValidatedSkillRow> SkillRows { get; init; }
    public IEnumerable<ImportRowResult> Results => EmployeeRows.Select(r => r.Result)
        .Concat(SkillRows.Select(row => row.Result));
}