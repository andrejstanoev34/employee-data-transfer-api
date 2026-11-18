using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.ToTable("Organisations");
        
        builder.HasKey(o=>o.Id);
        
        builder.Property(o=>o.Name)
            .IsRequired()
            .HasMaxLength(200);


        var organisations = new Organisation[]
        {
            new Organisation()
            {
                Id = Guid.Parse("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                Name = "Shelby Limited Co."
            },
            new Organisation()
            {
                Id = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                Name = "Dunder Mifflin Paper Co."
            }
        };
        
        builder.HasData(
            organisations
        );
    }
}