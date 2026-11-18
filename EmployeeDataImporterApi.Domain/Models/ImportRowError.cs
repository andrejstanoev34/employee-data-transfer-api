using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Models;

public class ImportRowError
{
    public Guid Id {get; set;}
    public string Field {get; set;}
    public string Message {get; set;}
    public string Value {get; set;}
    public ErrorType ErrorType {get; set;}
    
    public Guid ImportRowResultId {get; set;}
    public ImportRowResult? ImportRowResult {get; set;}
}