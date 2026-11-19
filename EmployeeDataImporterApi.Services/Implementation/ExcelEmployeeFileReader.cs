using ClosedXML.Excel;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Domain.Enums;
using EmployeeDataImporterApi.Domain.Models;
using EmployeeDataImporterApi.Services.Interface;

namespace EmployeeDataImporterApi.Services.Implementation;

public class ExcelEmployeeFileReader : IEmployeeFileReader
{
    public ImportRunDto ReadFile(Stream stream)
    {
        using var workBook = new XLWorkbook(stream);
        var workSheets = workBook.Worksheets;
        var numSheets = workSheets.Count;

        if (numSheets == 1)
        {
            var ws = workSheets.First();
            if (ws.IsEmpty())
            {
                return null;
            }
        }

        for (int i = 1; i <= numSheets; i++)
        {
            var ws = workBook.Worksheet(i);
            ProcessSheet(ws);
        }

        return null;
    }

    private ImportRunDto ProcessSheet(IXLWorksheet worksheet)
    {
        if (worksheet.Name.ToLower().Contains("skill"))
        {
            return ProcessSkills(worksheet);
        }
        else
        {
            return ProcessEmployees(worksheet);
        }
    }

    private ImportRunDto ProcessEmployees(IXLWorksheet worksheet)
    {
        var requiredHeaders = new List<string> {"employeenumber", "firstname", "lastname", "email", "dateofbirth", "hiredate", "departmentcode", "roletitle", "officecity", "salary", "phone", "manageremployeenumber", "notes" };
        
        var headers = new Dictionary<string, int>();
        
        var headersRow = worksheet.Row(1);

        for (int col = 1; col <= headersRow.LastCellUsed().Address.ColumnNumber; col++)
        {
            headers[headersRow.Cell(col).GetString().Trim().ToLower()] = col;
        }

        foreach (var h in requiredHeaders)
        {
            if (!headers.ContainsKey(h))
            {
                return null;
            }
        }

        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;

        for (int row = 2; row <= lastRow; row++)
        {
            var rowErrors = new List<ImportRowError>();
            var employeeNumber = worksheet.Cell(row, headers["employeenumber"]).GetString().Trim();
            if (string.IsNullOrEmpty(employeeNumber))
            {
                rowErrors.Add(
                    new ImportRowError()
                    {
                        Field = "Employee Number",
                        Message = "Employee Number is required",
                        Value = "",
                        ErrorType = ErrorType.MissingRequiredField
                    }    
                );
            }
            var firstName = worksheet.Cell(row, headers["firstname"]).GetString().Trim();
            if (string.IsNullOrEmpty(firstName))
            {
                rowErrors.Add(
                    new ImportRowError()
                    {
                        Field = "First Name",
                        Message = "First Name is required",
                        Value = "",
                        ErrorType = ErrorType.MissingRequiredField
                    }    
                );
            }
            var lastName = worksheet.Cell(row, headers["lastname"]).GetString().Trim();
            if (string.IsNullOrEmpty(lastName))
            {
                rowErrors.Add(
                    new ImportRowError()
                    {
                        Field = "Last Name",
                        Message = "Last Name is required",
                        Value = "",
                        ErrorType = ErrorType.MissingRequiredField
                    }    
                );
            }
        }

        return null;
    }
    
    private ImportRunDto ProcessSkills(IXLWorksheet worksheet)
    {
        return null;
    }
}