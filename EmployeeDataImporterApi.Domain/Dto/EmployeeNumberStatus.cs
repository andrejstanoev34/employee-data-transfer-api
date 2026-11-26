using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Dto;

public class EmployeeNumberStatus
{
    public string EmployeeNumber  { get; set; }
    public ImportRowStatus Status   { get; set; }

    public EmployeeNumberStatus(string employeeNumber, ImportRowStatus status)
    {
        EmployeeNumber = employeeNumber;
        Status = status;
    }
}