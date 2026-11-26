namespace EmployeeDataImporterApi.Domain.Dto;

public class TenureCohortsDto
{
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int HireYear { get; set; }
    public int NumberOfEmployees { get; set; }
    public int RunningTotal { get; set; }

    public static TenureCohortsDto Create(Guid DepartmentId, string DepartmentName, int HireYear, int NumberOfEmployees, int RunningTotal)
    {
        return new TenureCohortsDto()
        {
            DepartmentId = DepartmentId,
            DepartmentName = DepartmentName,
            HireYear = HireYear,
            NumberOfEmployees = NumberOfEmployees,
            RunningTotal = RunningTotal
        };
    }
}