using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Enums;

namespace EmployeeDataImporterApi.Domain.Models;

public class Employee: BaseEntity
{
    public string EmployeeNumber {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public DateOnly? DateOfBirth {get; set;}
    public DateOnly HireDate {get; set;}
    public decimal? Salary {get; set;}
    public string? Phone { get; set; }
    public string? Notes {get; set;}
    public RoleLevel? RoleLevel {get; set;}
    
    public Guid? ManagerId {get; set;}
    public Employee? Manager {get; set;}
    public ICollection<Employee> Employees {get; set;} = new List<Employee>();
    
    public ICollection<Skill> Skills {get; set;} = new List<Skill>();
    
    public Guid RoleId {get; set;}
    public Role? Role {get; set;}
    
    public Guid DepartmentId {get; set;}
    public Department? Department {get; set;}
    
    public Guid OfficeId {get; set;}
    public Office? Office {get; set;}
}