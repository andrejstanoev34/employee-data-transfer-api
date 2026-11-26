using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class ImportRowErrorConfiguration : IEntityTypeConfiguration<ImportRowIssue>
{
    public void Configure(EntityTypeBuilder<ImportRowIssue> builder)
    {
        builder.ToTable("ImportRowErrors");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Field)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(i => i.Message)
            .HasMaxLength(500);
        builder.Property(i => i.Value)
            .HasMaxLength(200);
        builder.Property(i => i.IssueType)
            .HasConversion<string>();
        
        builder.HasOne(i=>i.ImportRowResult)
            .WithMany(r=>r.Issues)
            .HasForeignKey(i=>i.ImportRowResultId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}