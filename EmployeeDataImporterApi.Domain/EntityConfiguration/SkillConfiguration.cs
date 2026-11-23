using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("Skills");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(100)
            .IsRequired();

        var skills = new Skill[]
        {
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000001"), Name = "Leadership", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000002"), Name = "Public Speaking", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000003"), Name = "Sales", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000004"), Name = "First Aid", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000005"), Name = "Customer Service", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000006"), Name = "Accounting", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000007"), Name = "Chili Cooking", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000008"), Name = "Inventory Management",
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000009"), Name = "Forklift Operation", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000010"), Name = "Basketball", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000011"), Name = "Negotiation", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000012"), Name = "Excel", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000013"), Name = "Bookkeeping", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000014"), Name = "Driving", IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000015"), Name = "Security", IsDeleted = false,
                CreatedAt = SeedData.SeedDate, UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000016"), Name = "Logistics Planning", IsDeleted = false,
                CreatedAt = SeedData.SeedDate, UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000017"), Name = "Whiskey Tasting", IsDeleted = false,
                CreatedAt = SeedData.SeedDate, UpdatedAt = SeedData.SeedDate
            },
            new Skill
            {
                Id = Guid.Parse("a1000001-0000-4000-8000-000000000018"), Name = "Horse Racing", IsDeleted = false,
                CreatedAt = SeedData.SeedDate, UpdatedAt = SeedData.SeedDate
            }
        };

        builder.HasData(skills);
    }
}