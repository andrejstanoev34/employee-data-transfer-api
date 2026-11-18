using System.Linq.Expressions;
using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDataImporterApi.Domain;

public class EmployeeDataImporterContext: DbContext
{
    
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeSkill> EmployeeSkills { get; set; }
    public DbSet<ImportRowError> ImportRowErrors { get; set; }
    public DbSet<ImportRowResult> ImportRowResults { get; set; }
    public DbSet<ImportRun>  ImportRuns { get; set; }
    public DbSet<Office>  Offices { get; set; }
    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Skill> Skills { get; set; }
    
    public EmployeeDataImporterContext (DbContextOptions<EmployeeDataImporterContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeDataImporterContext).Assembly);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            // builds:  e => !e.IsDeleted
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var isDeleted = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var filter = Expression.Lambda(Expression.Not(isDeleted), parameter);

            entityType.SetQueryFilter(filter);
        }
        base.OnModelCreating(modelBuilder);
    }
}