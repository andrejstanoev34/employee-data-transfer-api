using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        
        builder.HasKey(r=>r.Id);
        
        builder.Property(r=>r.Title)
            .IsRequired()
            .HasMaxLength(100);

        var roles = new Role[]
        {
            new Role()
            {
                Id = Guid.Parse("813bf3a4-afa1-44a2-945c-8c7f441cd3a0"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                Title = "Manager"
            },
            new Role()
            {
                Id = Guid.Parse("9a66ca52-56e5-4328-adca-1c29186c252d"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                Title = "Associate"
            },
            new Role()
            {
                Id = Guid.Parse("560bb0e8-1805-41b5-ab2f-3bb615b8c062"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                Title = "Team Lead"
            },
            new Role()
            {
                Id = Guid.Parse("0d92a7ff-df8c-49ad-9d4a-123ed27b6b1d"),
                IsDeleted = false,
                CreatedAt = SeedData.SeedDate,
                UpdatedAt = SeedData.SeedDate,
                Title = "Bookmaker"
            },
        };
        
        builder.HasData(roles);
    }
}