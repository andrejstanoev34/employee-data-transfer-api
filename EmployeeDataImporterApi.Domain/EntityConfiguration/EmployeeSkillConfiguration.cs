using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeDataImporterApi.Domain.EntityConfiguration;

public class EmployeeSkillConfiguration : IEntityTypeConfiguration<EmployeeSkill>
{
    public void Configure(EntityTypeBuilder<EmployeeSkill> builder)
    {
        builder.ToTable("EmployeeSkill");

        builder.HasKey(s => s.Id);
        
        builder.HasOne(es=>es.Skill)
            .WithMany(s=>s.Employees)
            .HasForeignKey(es=>es.SkillId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(es=>es.Employee)
            .WithMany(e=>e.Skills)
            .HasForeignKey(es=>es.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}