using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;
using EmployeeDataImporterApi.Repository;

namespace EmployeeDataImporterApi.Services.Implementation;

public class EmployeeDataValidator
{
    
    private readonly Repository<Employee> _employeeRepository;

    public EmployeeDataValidator(Repository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    // treba da prima Organisation Id kako parametar funkcijata
    public async Task<string> ValidateAsync(ImportedData importedData)
    {
        var employees = await _employeeRepository.GetAllAsync(selector: x => x);
        //treba filter po organisation ID da se dodade
        var employeeNumbers = (await _employeeRepository.GetAllAsync(selector: x => x.EmployeeNumber)).ToList();
        var emails = (await _employeeRepository.GetAllAsync(selector:x=>x.Email) ).ToList();
        
        var importRun = new ImportRun();
        
        var employeeRows = importedData.ImportedEmployeesData.ImportedEmployees;
        
        foreach (var row in employeeRows)
        {
            var rowResult = new ImportRowResult();
            rowResult.RowNumber = row.RowNumber;
            
            CheckRequiredField(row.EmployeeNumber, "EmployeeNumber", "Employee number is required", rowResult.Errors);
            CheckRequiredField(row.FirstName, "FirstName", "First name is required", rowResult.Errors);
            CheckRequiredField(row.LastName, "LastName", "Last name is required", rowResult.Errors);
            CheckRequiredField(row.Email, "Email", "Email is required", rowResult.Errors);
            CheckRequiredField(row.HireDate, "HireDate", "HireDate is required", rowResult.Errors);
            CheckRequiredField(row.DepartmentCode, "DepartmentCode", "DepartmentCode is required", rowResult.Errors);
            CheckRequiredField(row.RoleTitle, "RoleTitle", "RoleTitle is required", rowResult.Errors);
            
            rowResult.EmployeeNumber = row.EmployeeNumber;
            
            if (rowResult.Errors.Any(e => e.ErrorType.Equals(ErrorType.MissingRequiredField)))
            {
                rowResult.Status = ImportStatus.Skipped;
                importRun.ImportRowResults.Add(rowResult);
                continue;
            }

            if (employeeNumbers.Contains(row.EmployeeNumber!))
            {
                rowResult.Status = ImportStatus.Skipped;
                rowResult.Errors.Add(new ImportRowError()
                {
                    Id = new Guid(),
                    Field = "EmployeeNumber",
                    ErrorType = ErrorType.DuplicateEmployeeNumber,
                    Message = "Employee number already exists in this company.",
                    Value = row.EmployeeNumber!
                });
                importRun.ImportRowResults.Add(rowResult);
            }

            if (emails.Contains(row.Email!))
            {
                rowResult.Status = ImportStatus.Skipped;
                rowResult.Errors.Add(new ImportRowError()
                {
                    Id = new Guid(),
                    Field = "Email",
                    ErrorType = ErrorType.DuplicateEmail,
                    Message = "Email already exists.",
                    Value = row.EmployeeNumber!
                });
                importRun.ImportRowResults.Add(rowResult);
            }
            
            if (rowResult.Errors.Any(e => e.ErrorType.Equals(ErrorType.DuplicateEmail) || e.ErrorType.Equals(ErrorType.DuplicateEmployeeNumber)))
            {
                continue;
            }
            
            
            
            var employee = new Employee();
            employee.EmployeeNumber = row.EmployeeNumber!;
            employee.FirstName = row.FirstName!;
            employee.LastName = row.LastName!;
            employee.Email = row.Email!;
            employee.HireDate = GetDate(row.HireDate!);
            
        }

        return "";
    }

    private void CheckRequiredField(string? value, string fieldName, string message, List<ImportRowError> errors)
    {
        if (string.IsNullOrEmpty(value))
        {
            var importError = new ImportRowError()
            {
                Id = new Guid(),
                Field = fieldName,
                Message = message,
                Value = "null",
                ErrorType = ErrorType.MissingRequiredField
            };
            errors.Add(importError);
        }
    }

    private DateOnly GetDate(string date)
    {
        return DateOnly.Parse(date);
    }
    
}