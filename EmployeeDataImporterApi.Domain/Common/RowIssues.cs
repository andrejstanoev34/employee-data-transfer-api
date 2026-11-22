using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;

namespace EmployeeDataImporterApi.Domain.Common;

public class RowIssues
{
    private readonly ImportRowResult _result;
    public bool MustSkip { get; private set; }
    public RowIssues(ImportRowResult result) => _result = result;

    public void Required(string field, ErrorType type, string message, string? original)
    {
        MustSkip = true;
        _result.Status = ImportStatus.Skipped;
        Add(field, type, message, original);
    }

    public void Optional(string field, ErrorType type, string message, string? original) =>
        Add(field, type, message, original);

    private void Add(string field, ErrorType type, string message, string? original) =>
        _result.Errors.Add(new ImportRowError
        {
            Field = field, ErrorType = type, Message = message, Value = original ?? ""
        });
}