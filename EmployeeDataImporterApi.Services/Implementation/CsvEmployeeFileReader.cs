using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Dto;

namespace EmployeeDataImporterApi.Services.Implementation;

public class CsvEmployeeFileReader : FileReaderBase
{
    public override void ReadFile(Stream stream, ImportedData data, string fileName)
    {
        
        using var csv = CreateCsv(stream);
        if (!csv.Read() || !csv.ReadHeader())
        {
            data.Messages.Add("Empty file");
            return;   
        }
        
        var presentHeaders = csv.HeaderRecord!.Select(h => h.Trim().ToLower()).ToHashSet();
        

        if (HasHeaders(presentHeaders, data.Messages, "skills",
                "employeenumber", "firstname", "lastname", "email", "dateofbirth",
                "hiredate", "departmentcode", "roletitle", "officecity", "salary",
                "phone", "manageremployeenumber"))
        {
            ReadEmployees(csv, data.ImportedEmployeesData);
        }
        else if(HasHeaders(presentHeaders, data.Messages, "skills",
                    "employeenumber", "firstname", "lastname", "dateofbirth", "skillname"))
        {
            ReadSkills(csv, data.ImportedSkillsData);
        }
        else
        {
            data.Messages.Add($"The file {fileName} does not contain the right headers");
        }
    }

    public override bool CanReadFile(string extension) =>
        extension.Equals(".csv", StringComparison.OrdinalIgnoreCase);

    private void ReadEmployees(CsvReader csv, ImportedEmployeesData data)
    {
        while (csv.Read())
        {
            string? employeeNumber = Get(csv, "employeenumber");
            string? firstName = Get(csv, "firstname");
            string? lastName = Get(csv, "lastname");
            string? email = Get(csv, "email");
            string? dateOfBirth = Get(csv, "dateofbirth");
            string? hiredate = Get(csv, "hiredate");
            string? departmentcode = Get(csv, "departmentcode");
            string? roleTitle = Get(csv, "roletitle");
            string? officecity = Get(csv, "officecity");
            string? salary = Get(csv, "salary");
            string? phone = Get(csv, "phone");
            string? managerEmployeeNumber = Get(csv, "manageremployeenumber");
            string? notes = Get(csv, "notes");
            
            if(Util.IsEmpty(employeeNumber, firstName, lastName, email, dateOfBirth, hiredate, departmentcode, roleTitle,
                   officecity, salary, phone, managerEmployeeNumber, notes)) continue;
            
            var emp = new ImportedEmployee
            {
                RowNumber = csv.Parser.Row,          
                EmployeeNumber = employeeNumber,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dateOfBirth,
                HireDate = hiredate,
                DepartmentCode = departmentcode,
                RoleTitle = roleTitle,
                OfficeCity = officecity,
                Salary = salary,
                Phone = phone,
                ManagerEmployeeNumber = managerEmployeeNumber,
                Notes = notes           
            };

            
            data.ImportedEmployees.Add(emp);
        }
        data.TotalEmployeeRows = data.ImportedEmployees.Count;
    }
    
    private void ReadSkills(CsvReader csv, ImportedSkillsData data)
    {
        while (csv.Read())
        {
            
            string? employeeNumber = Get(csv, "employeenumber");
            string? firstName = Get(csv, "firstname");
            string? lastName = Get(csv, "lastname");
            string? dateOfBirth = Get(csv, "dateofbirth");
            string? skillName = Get(csv, "skillname");
            
            if (Util.IsEmpty(employeeNumber, firstName, lastName, dateOfBirth, skillName))
            {
                continue;
            }
            
            var skill = new ImportedSkill
            {
                RowNumber = csv.Parser.Row,
                EmployeeNumber = Get(csv, "employeenumber"),
                FirstName = Get(csv, "firstname"),
                LastName = Get(csv, "lastname"),
                DateOfBirth = Get(csv, "dateofbirth"),
                SkillName = Get(csv, "skillname")
            };

            data.ImportedSkills.Add(skill);
        }
        data.TotalSkillRows = data.ImportedSkills.Count;
    }
    
    private CsvReader CreateCsv(Stream stream)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.Trim().ToLower(),
            HeaderValidated = null,     
            MissingFieldFound = null,   
            BadDataFound = null,
            DetectDelimiter = true
        };
        return new CsvReader(new StreamReader(stream), config);
    }
    
    private string? Get(CsvReader csv, string header)
    {
        if (!csv.TryGetField<string>(header, out var value)) return null;
        value = value?.Trim();
        return string.IsNullOrEmpty(value) ? null : value;
    }
}