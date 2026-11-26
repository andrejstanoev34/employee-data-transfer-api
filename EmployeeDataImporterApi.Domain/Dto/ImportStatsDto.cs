namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportStatsDto
{
    public List<ErrorCount> ErrorCounts { get; set; } = new List<ErrorCount>();

    public List<OrganisationSkippedPartialCount> OrganisationSkippedPartial { get; set; } =
        new List<OrganisationSkippedPartialCount>();
}