using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class ImportRowResultConfiguration : IEntityTypeConfiguration<ImportRowResult>
{
    public void Configure(EntityTypeBuilder<ImportRowResult> builder)
    {
        builder.ToTable("ImportRowResults");
        
        builder.HasKey(i=>i.Id);

        builder.Property(i => i.RowNumber)
            .IsRequired();

        builder.Property(i => i.EmployeeNumber)
            .HasMaxLength(100)       
            .IsRequired(false);

        builder.Property(i => i.RowStatus)
            .HasConversion<string>();
        
        builder.HasOne(i=>i.ImportRun)
            .WithMany(r=>r.ImportRowResults)
            .HasForeignKey(i=>i.ImportRunId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}