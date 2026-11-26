namespace EmployeeDataImporterApi.Domain.Dto;

public class ImportStatsDto
{
    public List<IssueCount> IssueCounts { get; set; } = new List<IssueCount>();

    public List<OrganisationSkippedPartialCount> OrganisationSkippedPartial { get; set; } =
        new List<OrganisationSkippedPartialCount>();
}