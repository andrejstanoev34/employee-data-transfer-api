using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using EmployeeDataImporterApi.Domain.Common;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Services.Interface;

namespace EmployeeDataImporterApi.Services.Implementation;

public class CsvEmployeeFileReader : IEmployeeFileReader
{
    public void ReadFile(Stream stream, ImportedData data, string fileName)
    {
        if (fileName.Contains("employees"))
        {
            ReadEmployees(stream, data.ImportedEmployeesData);
        }
        else
        {
            ReadSkills(stream, data.ImportedSkillsData);
        }
    }

    public bool CanReadFile(string extension) =>
        extension.Equals(".csv", StringComparison.OrdinalIgnoreCase);

    private void ReadEmployees(Stream stream, ImportedEmployeesData data)
    {
        using var csv = CreateCsv(stream);

        if (!csv.Read() || !csv.ReadHeader())
        {
            data.Message.Add("Empty employee file");
            return;   
        }
        
        if (!HasHeaders(csv, data.Message,
                "employeenumber", "firstname", "lastname", "email", "dateofbirth",
                "hiredate", "departmentcode", "roletitle", "officecity", "salary",
                "phone", "manageremployeenumber"))
            return;

        while (csv.Read())
        {
            var emp = new ImportedEmployee
            {
                RowNumber = csv.Parser.Row,          
                EmployeeNumber = Get(csv, "employeenumber"),
                FirstName = Get(csv, "firstname"),
                LastName = Get(csv, "lastname"),
                Email = Get(csv, "email"),
                DateOfBirth = Get(csv, "dateofbirth"),
                HireDate = Get(csv, "hiredate"),
                DepartmentCode = Get(csv, "departmentcode"),
                RoleTitle = Get(csv, "roletitle"),
                OfficeCity = Get(csv, "officecity"),
                Salary = Get(csv, "salary"),
                Phone = Get(csv, "phone"),
                ManagerEmployeeNumber = Get(csv, "manageremployeenumber"),
                Notes = Get(csv, "notes")            
            };

            if (Util.IsEmpty(emp)) continue;
            data.ImportedEmployees.Add(emp);
        }
        data.TotalEmployeeRows = data.ImportedEmployees.Count;
    }
    
    private void ReadSkills(Stream stream, ImportedSkillsData data)
    {
        using var csv = CreateCsv(stream);

        if (!csv.Read() || !csv.ReadHeader())
        {
            data.Message.Add("Empty skills file");
            return;
        }

        if (!HasHeaders(csv, data.Message,
                "employeenumber", "firstname", "lastname", "dateofbirth", "skillname"))
            return;

        while (csv.Read())
        {
            var skill = new ImportedSkill
            {
                RowNumber = csv.Parser.Row,
                EmployeeNumber = Get(csv, "employeenumber"),
                FirstName = Get(csv, "firstname"),
                LastName = Get(csv, "lastname"),
                DateOfBirth = Get(csv, "dateofbirth"),
                SkillName = Get(csv, "skillname")
            };

            if (IsEmpty(skill)) continue;        
            data.ImportedSkills.Add(skill);
        }
        data.TotalSkillRows = data.ImportedSkills.Count;
    }

    private bool IsEmpty(ImportedSkill s) =>
        new[] { s.EmployeeNumber, s.FirstName, s.LastName, s.DateOfBirth, s.SkillName }
            .All(v => v == null);
    
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
    
    private bool HasHeaders(CsvReader csv, List<string> messages, params string[] required)
    {
        var present = csv.HeaderRecord!.Select(h => h.Trim().ToLower()).ToHashSet();
        var missing = required.Where(h => !present.Contains(h)).ToList();
        foreach (var h in missing)
            messages.Add($"The header {h} is missing. Importing stopped");
        return missing.Count == 0;
    }
    
    private string? Get(CsvReader csv, string header)
    {
        if (!csv.TryGetField<string>(header, out var value)) return null;
        value = value?.Trim();
        return string.IsNullOrEmpty(value) ? null : value;
    }
}