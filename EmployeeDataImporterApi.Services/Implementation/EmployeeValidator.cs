using System.Globalization;
using DocumentFormat.OpenXml.Office2010.Excel;
using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;

namespace EmployeeDataImporterApi.Services.Implementation;

public class EmployeeValidator
{
    public List<ValidatedEmployeeRow> ValidateEmployees(ImportedEmployeesData importedEmployeesData, ReferenceData refData)
    {
        var rows = new List<ValidatedEmployeeRow>();

        foreach (var row in importedEmployeesData.ImportedEmployees)
        {
            var validated = ValidateEmployee(row, refData);

            if (validated.Employee != null)
            {
                refData.TakenEmployeeNumbers.Add(validated.Employee.EmployeeNumber);
                refData.TakenEmails.Add(validated.Employee.Email);
            }
            rows.Add(validated);
        }

        MarkManagerCycles(rows);
        LinkManagers(rows);

        return rows;
    }
    
    
    private ValidatedEmployeeRow ValidateEmployee(ImportedEmployee row, ReferenceData refData)
    {
        var result = new ImportRowResult { RowNumber = row.RowNumber, EmployeeNumber = Util.Clean(row.EmployeeNumber) };
        var issues = new RowIssues(result);
        
        var employeeNumber = Util.Clean(row.EmployeeNumber);
        var firstName = Util.Clean(row.FirstName);
        var lastName = Util.Clean(row.LastName);
        var email = Util.Clean(row.Email)?.ToLowerInvariant();
        var departmentCode = Util.Clean(row.DepartmentCode);
        var roleTitle = Util.Clean(row.RoleTitle);
        var hireDateText = Util.Clean(row.HireDate);
        var dateOfBirthText = Util.Clean(row.DateOfBirth);
        var salaryText = Util.Clean(row.Salary);
        var phoneText = Util.Clean(row.Phone);
        var officeCity = Util.Clean(row.OfficeCity);
        var managerNumber = Util.Clean(row.ManagerEmployeeNumber);

        //required
        if (employeeNumber == null)
            issues.Required("EmployeeNumber", ErrorType.MissingRequiredField, "Employee number is required", row.EmployeeNumber);
        else if (refData.TakenEmployeeNumbers.Contains(employeeNumber))
            issues.Required("EmployeeNumber", ErrorType.DuplicateEmployeeNumber, "Employee number already exists", row.EmployeeNumber);

        if (firstName == null)
            issues.Required("FirstName", ErrorType.MissingRequiredField, "First name is required", row.FirstName);
        if (lastName == null)
            issues.Required("LastName", ErrorType.MissingRequiredField, "Last name is required", row.LastName);

        if (email == null)
            issues.Required("Email", ErrorType.MissingRequiredField, "Email is required", row.Email);
        else if (!Util.EmailRegex.IsMatch(email))
            issues.Required("Email", ErrorType.EmailInvalid, "Email is not a valid address", row.Email);
        else if (refData.TakenEmails.Contains(email))
            issues.Required("Email", ErrorType.DuplicateEmail, "Email already exists", row.Email);

        Guid departmentId = default;
        if (departmentCode == null)
            issues.Required("DepartmentCode", ErrorType.MissingRequiredField, "Department code is required", row.DepartmentCode);
        else if (!refData.DepartmentsByCode.TryGetValue(departmentCode, out departmentId))
            issues.Required("DepartmentCode", ErrorType.DepartmentUnknown, "Department code does not exist", row.DepartmentCode);

        Guid roleId = default;
        var level = RoleLevel.Mid;
        if (roleTitle == null)
            issues.Required("RoleTitle", ErrorType.MissingRequiredField, "Role title is required", row.RoleTitle);
        else
        {
            var (baseTitle, parsedLevel) = Util.SplitRoleTitle(roleTitle);
            level = parsedLevel;
            if (!refData.RolesByTitle.TryGetValue(baseTitle, out roleId))
                issues.Required("RoleTitle", ErrorType.RoleUnknown, "Role title does not match any known role", row.RoleTitle);
        }

        DateOnly hireDate = default;
        if (hireDateText == null)
            issues.Required("HireDate", ErrorType.MissingRequiredField, "Hire date is required", row.HireDate);
        else
        {
            var parsed = Util.ParseDate(hireDateText);
            if (parsed == null)
                issues.Required("HireDate", ErrorType.HireDateInvalid, "Hire date is not a valid date", row.HireDate);
            else if (parsed > refData.Today)
                issues.Required("HireDate", ErrorType.HireDateInFuture, "Hire date is in the future", row.HireDate);
            else
                hireDate = parsed.Value;
        }

        //optional
        DateOnly? dateOfBirth = null;
        if (dateOfBirthText != null)
        {
            dateOfBirth = Util.ParseDate(dateOfBirthText);
            if (dateOfBirth == null)
                issues.Optional("DateOfBirth", ErrorType.DateOfBirthInvalid, "Date of birth is not a valid date", row.DateOfBirth);
            else if (dateOfBirth.Value.AddYears(16) > refData.Today)
            {
                issues.Optional("DateOfBirth", ErrorType.DateOfBirthTooYoung, "Person would be under 16", row.DateOfBirth);
                dateOfBirth = null;
            }
        }

        decimal? salary = null;
        if (salaryText != null)
        {
            if (decimal.TryParse(salaryText, NumberStyles.Number, CultureInfo.InvariantCulture, out var s) && s > 0)
                salary = s;
            else
                issues.Optional("Salary", ErrorType.SalaryInvalid, "Salary must be a positive number", row.Salary);
        }

        string? phone = null;
        if (phoneText != null)
        {
            if (Util.IsValidPhone(phoneText)) phone = phoneText;
            else issues.Optional("Phone", ErrorType.PhoneInvalid, "Phone number is not valid", row.Phone);
        }

        Guid? officeId = null;
        if (officeCity != null)
        {
            if (refData.OfficesByCity.TryGetValue(officeCity, out var oid)) officeId = oid;
            else issues.Optional("OfficeCity", ErrorType.OfficeUnknown, "No office in this city", row.OfficeCity);
        }
        
        
        result.Status = issues.MustSkip ? ImportStatus.Skipped
                      : result.Errors.Count > 0 ? ImportStatus.Partial
                      : ImportStatus.Imported;
        
        result.FileType = FileType.Employee;
        
        if (issues.MustSkip)
            return new ValidatedEmployeeRow { Result = result };

        var employee = new Employee
        {
            OrganisationId = refData.OrganisationId,
            EmployeeNumber = employeeNumber!,
            FirstName = firstName!,
            LastName = lastName!,
            Email = email!,
            HireDate = hireDate,
            DateOfBirth = dateOfBirth,
            Salary = salary,
            Phone = phone,
            Notes = Util.Clean(row.Notes),
            RoleId = roleId,
            RoleLevel = level,
            DepartmentId = departmentId,
            OfficeId = officeId
        };

        return new ValidatedEmployeeRow { Result = result, Employee = employee, ManagerEmployeeNumber = managerNumber };
    }
    
    private void MarkManagerCycles(List<ValidatedEmployeeRow> rows)
    {
        var accepted = rows.Where(r => r.Employee != null)
            .ToDictionary(r => r.Employee!.EmployeeNumber, StringComparer.OrdinalIgnoreCase);

        var managerOf = accepted.Values
            .Where(r => r.ManagerEmployeeNumber != null && accepted.ContainsKey(r.ManagerEmployeeNumber))
            .ToDictionary(
                r => r.Employee!.EmployeeNumber,
                r => accepted[r.ManagerEmployeeNumber!].Employee!.EmployeeNumber);

        var visited = new HashSet<string>();
        var onPath = new HashSet<string>();
        var inCycle = new HashSet<string>();

        foreach (var start in managerOf.Keys)
        {
            var path = new List<string>();
            string? current = start;

            while (current != null && visited.Add(current))
            {
                path.Add(current);
                managerOf.TryGetValue(current, out current);
            }

            if (current != null && path.Contains(current))
                foreach (var n in path.Skip(path.IndexOf(current)))
                    inCycle.Add(n);
        }

        foreach (var number in inCycle)
        {
            var row = accepted[number];
            row.Employee = null;
            row.Result.Status = ImportStatus.Skipped;
            row.Result.Errors.Add(new ImportRowError
            {
                Field = "ManagerEmployeeNumber",
                ErrorType = ErrorType.ManagerCycle,
                Message = "Manager chain forms a cycle",
                Value = row.ManagerEmployeeNumber ?? ""
            });
        }
    }

    private void LinkManagers(List<ValidatedEmployeeRow> rows)
    {
        var saved = rows.Where(r => r.Employee != null)
            .ToDictionary(r => r.Employee!.EmployeeNumber, r => r.Employee!, StringComparer.OrdinalIgnoreCase);

        foreach (var row in rows.Where(r => r.Employee != null && r.ManagerEmployeeNumber != null))
        {
            if (saved.TryGetValue(row.ManagerEmployeeNumber!, out var manager))
                row.Employee!.Manager = manager;
            else
            {
                row.Result.Status = ImportStatus.Partial;
                row.Result.Errors.Add(new ImportRowError
                {
                    Field = "ManagerEmployeeNumber",
                    ErrorType = ErrorType.ManagerUnknown,
                    Message = "Manager does not match any saved employee",
                    Value = row.ManagerEmployeeNumber!
                });
            }
        }
    }
}