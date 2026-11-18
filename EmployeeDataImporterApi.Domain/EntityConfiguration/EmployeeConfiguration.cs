using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.EmployeeNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Phone)
            .HasMaxLength(50);

        builder.Property(e => e.Notes)
            .HasMaxLength(2000);

        builder.Property(e => e.Salary)
            .HasPrecision(18, 2);

        builder.Property(e => e.RoleLevel)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(e => e.EmployeeNumber).IsUnique();
        builder.HasIndex(e => e.Email).IsUnique();

        builder.HasOne(e => e.Manager)
            .WithMany(m => m.Employees)
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Role)
            .WithMany(r => r.Employees)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Office)
            .WithMany(o=>o.Employees)
            .HasForeignKey(e => e.OfficeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}