using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class ImportRunConfiguration : IEntityTypeConfiguration<ImportRun>
{
    public void Configure(EntityTypeBuilder<ImportRun> builder)
    {
        builder.ToTable("ImportRuns");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.FileName)
            .IsRequired()
            .HasMaxLength(260);

        builder.Property(r => r.StartTime)
            .IsRequired();

        builder.Property(r => r.EndTime);

        builder.Property(r => r.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.TotalRows)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(r => r.ProcessedRows)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.Property(r => r.Message)
            .HasDefaultValue(string.Empty)
            .HasMaxLength(250);
        
        builder.HasOne(r => r.Organisation)
            .WithMany(o=>o.ImportRuns)
            .HasForeignKey(r => r.OrganisationId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}