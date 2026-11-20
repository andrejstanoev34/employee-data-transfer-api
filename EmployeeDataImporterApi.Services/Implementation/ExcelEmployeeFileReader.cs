using System.Globalization;
using ClosedXML.Excel;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Services.Interface;

namespace EmployeeDataImporterApi.Services.Implementation;

public class ExcelEmployeeFileReader : IEmployeeFileReader
{
    public void ReadFile(Stream stream, ImportedData importedData, string fileName)
    {
        using var workbook = new XLWorkbook(stream);
        var workSheets = workbook.Worksheets;
        var numSheets = workSheets.Count;
        
        if (numSheets == 1)
        {
            var ws = workSheets.First();
            if (ws.IsEmpty())
            {
                importedData.AddMessage("Empty file");
                return;
            }
        }
        
        for (int i = 1; i <= numSheets; i++)
        {
            var ws = workbook.Worksheet(i);
            ReadSheet(ws, importedData);
        }
    }

    public bool CanReadFile(string extension) =>
        extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase);

    private void ReadSheet(IXLWorksheet worksheet, ImportedData data)
    {
        if (worksheet.Name.ToLower().Contains("skill"))
        {
            data.ImportedSkillsData = ReadSkills(worksheet);
        }
        else
        {
            data.ImportedEmployeesData = ReadEmployees(worksheet);
        }
    }

    private ImportedEmployeesData ReadEmployees(IXLWorksheet worksheet)
    {
        var data = new ImportedEmployeesData();
        
        var headers = GetHeaders(worksheet);

        if (!HasHeaders(headers, data.Message, "employees", "employeenumber", "firstname", "lastname", "email",
                "dateofbirth", "hiredate", "departmentcode", "roletitle", "officecity", "salary", "phone",
                "manageremployeenumber", "notes"))
        {
            return data;
        }

        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;

        for (int row = 2; row <= lastRow; row++)
        {
            string? employeeNumber = GetValue(worksheet, row, headers["employeenumber"]);
            string? firstName = GetValue(worksheet, row, headers["firstname"]);
            string? lastName = GetValue(worksheet, row, headers["lastname"]);
            string? email = GetValue(worksheet, row, headers["email"]); 
            string? dateOfBirth = GetValue(worksheet, row, headers["dateofbirth"]);
            string? hiredate = GetValue(worksheet, row, headers["hiredate"]);
            string? departmentcode = GetValue(worksheet, row, headers["departmentcode"]);
            string? roleTitle = GetValue(worksheet, row, headers["roletitle"]);
            string? officecity = GetValue(worksheet, row, headers["officecity"]);
            string? salary = GetValue(worksheet, row, headers["salary"]);
            string? phone = GetValue(worksheet, row, headers["phone"]);
            string? managerEmployeeNumber = GetValue(worksheet, row, headers["manageremployeenumber"]);
            string? notes = GetValue(worksheet, row, headers["notes"]);
            
            var emp = new ImportedEmployee()
            {
                RowNumber = row,
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
            //"employeenumber", "firstname", "lastname", "email", "dateofbirth", "hiredate", "departmentcode", "roletitle", "officecity", "salary", "phone", "manageremployeenumber", "notes" }
        }

        data.TotalEmployeeRows = lastRow - 1;
        
        return data;
    }


    private bool HasHeaders(Dictionary<string, int> headers, List<string> messages, string fileType, params string[] requiredHeaders)
    {
        var missingHeaders = requiredHeaders.Where(h => !headers.ContainsKey(h)).ToList();
        foreach (var h in missingHeaders)
        {
            messages.Add($"The header {h} is missing from {fileType} file.");
        }

        return missingHeaders.Count == 0;
    }
    
    private ImportedSkillsData ReadSkills(IXLWorksheet worksheet)
    {
        var data = new ImportedSkillsData();
        
        var headers = GetHeaders(worksheet);

        if (!HasHeaders(headers, data.Message, "skills", "employeenumber", "firstname", "lastname", "dateofbirth", "skillname"))
        {
            return data;
        }

        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;

        for (int row = 2; row <= lastRow; row++)
        {
            string? employeeNumber = GetValue(worksheet, row, headers["employeenumber"]);
            string? firstName = GetValue(worksheet, row, headers["firstname"]);
            string? lastName = GetValue(worksheet, row, headers["lastname"]);
            string? dateOfBirth = GetValue(worksheet, row, headers["dateofbirth"]);
            string? skillName = GetValue(worksheet, row, headers["skillname"]);
            
            data.ImportedSkills.Add(new ImportedSkill()
            {
                RowNumber = row,
                EmployeeNumber = employeeNumber,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                SkillName = skillName
            });
        }

        data.TotalSkillRows = lastRow - 1;
        
        return data;
    }
    
    private string? GetValue(IXLWorksheet worksheet, int row, int col)
    {
        var cell = worksheet.Cell(row, col);
        if (cell.IsEmpty()) return null;

        string value = cell.DataType switch
        {
            XLDataType.DateTime => cell.GetDateTime().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            XLDataType.Number   => cell.GetDouble().ToString("0.##########", CultureInfo.InvariantCulture),
            _                   => cell.GetString().Trim()
        };

        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    }

    private Dictionary<string, int> GetHeaders(IXLWorksheet worksheet)
    {
        var headers = new Dictionary<string, int>();
        
        var headersRow = worksheet.Row(1);

        for (int col = 1; col <= headersRow.LastCellUsed().Address.ColumnNumber; col++)
        {
            headers[headersRow.Cell(col).GetString().Trim().ToLower()] = col;
        }
        
        return headers;
    }
}