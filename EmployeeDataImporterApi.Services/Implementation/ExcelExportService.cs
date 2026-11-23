using ClosedXML.Excel;
using EmployeeDataImporterApi.Domain.Models;
using EmployeeDataImporterApi.Repository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDataImporterApi.Services.Implementation;


public class ExcelExportService
{
    
    private readonly Repository<ImportRun> _importRunRepository;

    public ExcelExportService(Repository<ImportRun> importRunRepository)
    {
        _importRunRepository = importRunRepository;
    }

    public async Task<byte[]> ExportToExcel(Guid importRunId)
    {
        var importRun = await _importRunRepository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == importRunId,
            include:x=>x.Include(i=>i.Organisation).Include(i=>i.ImportRowResults).ThenInclude(irr=>irr.Errors)
            );

        if (importRun == null)
        {
            throw new Exception("Import run not found, wrong import run id");
        }
        
        using var workBook = new XLWorkbook();
        // First worksheet
        var importRunWs = workBook.Worksheets.Add("ImportRun");

        var importRunHeaders = new[]
        {
            "ImportRunId", "FileName", "OrganisationName", "Status", "Message", "TotalRows", "ProcessedRows",
            "StartTime", "EndTIme"
        };
        
        ConfigureHeaders(importRunWs, importRunHeaders);
        
        importRunWs.Cell(2, 1).Value = importRun.Id.ToString();
        importRunWs.Cell(2, 2).Value = importRun.FileName;
        importRunWs.Cell(2, 3).Value = importRun.Organisation.Name;
        importRunWs.Cell(2, 4).Value = importRun.Status.ToString();
        importRunWs.Cell(2, 5).Value = importRun.Message;
        importRunWs.Cell(2, 6).Value = importRun.TotalRows.ToString();
        importRunWs.Cell(2, 7).Value = importRun.ProcessedRows.ToString();
        importRunWs.Cell(2, 8).Value = importRun.StartTime;
        importRunWs.Cell(2, 8).Value = importRun.EndTime;
        
        ConfigureWorksheet(importRunWs);
        // Seconds Worksheet
        var importRunResultsWs = workBook.Worksheets.Add("ImportRunResults");

        var importRunResultsHeaders = new[]
        {
            "ImportRunResultId", "RowNumber", "EmployeeNumber", "Status", "FileType"
        };
       
        ConfigureHeaders(importRunResultsWs, importRunResultsHeaders);
        
        int rowNumber = 2;
        
        foreach (var result in importRun.ImportRowResults)
        {
            importRunResultsWs.Cell(rowNumber, 1).Value = result.Id.ToString();
            importRunResultsWs.Cell(rowNumber, 2).Value = result.RowNumber;
            importRunResultsWs.Cell(rowNumber, 3).Value = result.EmployeeNumber;
            importRunResultsWs.Cell(rowNumber, 4).Value = result.Status.ToString();
            importRunResultsWs.Cell(rowNumber, 5).Value = result.FileType.ToString();
            
            rowNumber++;
        }
        ConfigureWorksheet(importRunResultsWs);
        // Third worksheet
        var importRunResultsIssuesWs = workBook.Worksheets.Add("ImportRunResultsIssues");

        var importRunResultsIssuesHeaders = new[]
        {
            "ImportRowErrorId", "ImportRowResultId", "Field", "Message", "Value", "ErrorType"
        };

        ConfigureHeaders(importRunResultsIssuesWs, importRunResultsIssuesHeaders);
        
        var importRunErrors = importRun.ImportRowResults.SelectMany(irr=>irr.Errors).ToList();

        int issueRowNumber = 2;
        
        foreach (var issue in importRunErrors)
        {
            importRunResultsIssuesWs.Cell(issueRowNumber, 1).Value = issue.Id.ToString();
            importRunResultsIssuesWs.Cell(issueRowNumber, 2).Value = issue.ImportRowResultId.ToString();
            importRunResultsIssuesWs.Cell(issueRowNumber, 3).Value = issue.Field;
            importRunResultsIssuesWs.Cell(issueRowNumber, 4).Value = issue.Message;
            importRunResultsIssuesWs.Cell(issueRowNumber, 5).Value = issue.Value;
            importRunResultsIssuesWs.Cell(issueRowNumber, 6).Value = issue.ErrorType.ToString();
            
            issueRowNumber++;
        }
        ConfigureWorksheet(importRunResultsIssuesWs);
        
        using var stream = new MemoryStream();
        workBook.SaveAs(stream);
        return stream.ToArray();
    }

    private void ConfigureWorksheet(IXLWorksheet ws)
    {
        ws.Columns().AdjustToContents();
        ws.RangeUsed()?.SetAutoFilter();
        ws.SheetView.FreezeRows(1);
    }

    private void ConfigureHeaders(IXLWorksheet ws, string[] headers)
    {
        
        for (int i = 0; i < headers.Length; i++)
        {
            ws.Cell(1, i+1).Value = headers[i];
        }
        
        var headerRange = ws.Range(1, 1, 1, headers.Length);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#4F46E5");
        headerRange.Style.Font.FontColor = XLColor.White;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
    }
}