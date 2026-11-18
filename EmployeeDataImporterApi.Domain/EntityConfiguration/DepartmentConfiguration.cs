using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Code);
        builder.Property(d => d.Name);

        builder.HasOne(d => d.Organisation)
            .WithMany(o => o.Departments)
            .HasForeignKey(d => d.OrganisationId)
            .OnDelete(DeleteBehavior.Restrict);

        var departments = new Department[]
        {
            new Department
            {
                Id = Guid.Parse("b462ba51-7b23-4ba9-a527-2aceff7a5b7e"), Code = "OPS", Name = "Operations",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece")
            },
            new Department
            {
                Id = Guid.Parse("e98228a5-2023-42c4-b24e-e2265f1277e7"), Code = "SEC", Name = "Secretarial",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece")
            },
            new Department
            {
                Id = Guid.Parse("811a5ac8-d3bf-4a89-911f-e2af1c58e5be"), Code = "HR", Name = "Human Resources",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece")
            },
            new Department
            {
                Id = Guid.Parse("c08ae784-b4ff-4cb3-ba68-339ee5d0fbed"), Code = "FIN", Name = "Finance",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece")
            },
            new Department
            {
                Id = Guid.Parse("d0256601-9d22-4972-928d-c1826747b481"), Code = "LOG", Name = "Logistics",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece")
            },
            new Department
            {
                Id = Guid.Parse("32a6098b-ca1b-485a-9414-2209209af7eb"), Code = "SECU", Name = "Security",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("08b581a5-7ae0-470b-aa31-d1a6a1ad5ece")
            },

            new Department
            {
                Id = Guid.Parse("3f2a9c1e-5b7d-4e86-a1c4-9d8e7f6b5a01"), Code = "ADM", Name = "Administration",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            },
            new Department
            {
                Id = Guid.Parse("7c4d1b2a-8e3f-4a95-b6d0-1f2e3a4b5c02"), Code = "SAL", Name = "Sales",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            },
            new Department
            {
                Id = Guid.Parse("a1e5f3c7-2d94-4b68-8c1a-6e7f8091a203"), Code = "ACC", Name = "Accounting",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            },
            new Department
            {
                Id = Guid.Parse("041d8943-30a3-4856-8638-3282e6d1c266"), Code = "HR", Name = "Human Resources",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            },
            new Department
            {
                Id = Guid.Parse("5d8b2e4f-9a1c-4c73-b0e6-3a4b5c6d7e04"), Code = "QA", Name = "Quality Assurance",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            },
            new Department
            {
                Id = Guid.Parse("e2f6a8b0-4c7d-4d19-9e35-7b8c9d0e1f05"), Code = "CS", Name = "Customer Service",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            },
            new Department
            {
                Id = Guid.Parse("94b3c5d7-1e2f-4a80-a6b9-0c1d2e3f4a06"), Code = "WHS", Name = "Warehouse",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                UpdatedAt = new DateTime(2026, 11, 18, 0, 0, 0),
                OrganisationId = Guid.Parse("49b43e69-ed71-4ac2-80cf-f73acec0e11f")
            }
        };

        builder.HasData(departments);
    }
}