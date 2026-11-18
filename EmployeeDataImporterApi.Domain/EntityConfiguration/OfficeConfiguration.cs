using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("Office");
        
        builder.HasKey(o=>o.Id);
        
        builder.Property(o=>o.City)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(o=>o.Country)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasOne(o=>o.Organisation)
            .WithMany(x=>x.Offices)
            .HasForeignKey(o=>o.OrganisationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(o => o.Departments)
            .WithMany(d => d.Offices)
            .UsingEntity(j => j.ToTable("OfficeDepartments"));

        var offices = new Office[]
        {
            new Office()
            {
                Id = Guid.Parse("4b0e864b-3db3-420c-96a3-4b90a2a213db"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                City = "London",
                Country = "England",
                OrganisationId = Guid.Parse("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece")
            },
            new Office()
            {
                Id = Guid.Parse("4b5cb182-6235-4a07-a1dd-dc978d1c3adf"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                City = "Birmingham",
                Country = "England",
                OrganisationId = Guid.Parse("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece")
            },
            new Office()
            {
                Id = Guid.Parse("77337cd5-ac62-45f3-b476-b8777f8738f4"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                City = "Manchester",
                Country = "England",
                OrganisationId = Guid.Parse("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece")
            },
            new Office()
            {
                Id = Guid.Parse("36c4af63-224c-4f5f-9b0c-cebb668fb02c"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                City = "Scranton",
                Country = "USA",
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            },
            new Office()
            {
                Id = Guid.Parse("f9e516df-59d7-42bc-a7ed-76e87ec1b0ac"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                City = "Stamford",
                Country = "USA",
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            },
            new Office()
            {
                Id = Guid.Parse("0c31f42f-0f8b-4664-a50e-69fcb0bd53cc"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                City = "Nashua",
                Country = "USA",
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            },
            new Office()
            {
                Id = Guid.Parse("f9b1b50c-a051-45ee-a0c7-a9362e26d4ea"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                City = "Utica",
                Country = "USA",
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            },
        };
        
        builder.HasData(offices);
    }
}