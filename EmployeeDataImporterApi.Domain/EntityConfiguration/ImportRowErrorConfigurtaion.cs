using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class ImportRowErrorConfiguration : IEntityTypeConfiguration<ImportRowError>
{
    public void Configure(EntityTypeBuilder<ImportRowError> builder)
    {
        builder.ToTable("ImportRowErrors");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Field)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(i => i.Message);
        builder.Property(i => i.Value);
        builder.Property(i => i.ErrorType)
            .HasConversion<string>();
        
        builder.HasOne(i=>i.ImportRowResult)
            .WithMany(r=>r.Errors)
            .HasForeignKey(i=>i.ImportRowResultId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}