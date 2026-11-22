using EmployeeDataImporterApi.Domain.Models;

namespace EmployeeDataImporterApi.Domain.Dto;

public class ValidationOutcome
{
    public required List<ValidatedEmployeeRow> Rows { get; init; }
    public IEnumerable<ImportRowResult> Results => Rows.Select(r => r.Result);
}