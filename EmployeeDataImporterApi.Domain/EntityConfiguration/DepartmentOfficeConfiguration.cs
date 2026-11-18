using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class DepartmentOfficeConfiguration : IEntityTypeConfiguration<DepartmentOffice>
{
    public void Configure(EntityTypeBuilder<DepartmentOffice> builder)
    {
        builder.ToTable("DepartmentOffice");
        
        builder.HasKey(o=>o.Id);
        
        builder.HasOne(o=>o.Department)
            .WithMany(x=>x.Offices)
            .HasForeignKey(o=>o.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(o=>o.Office)
            .WithMany(x=>x.Departments)
            .HasForeignKey(o=>o.OfficeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}