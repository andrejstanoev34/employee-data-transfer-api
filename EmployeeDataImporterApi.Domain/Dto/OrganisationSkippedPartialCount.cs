namespace EmployeeDataImporterApi.Domain.Dto;

public class OrganisationSkippedPartialCount
{
    public Guid OrganisationId { get; set; }
    public int SkippedCount { get; set; }
    public int PartialCount { get; set; }
}