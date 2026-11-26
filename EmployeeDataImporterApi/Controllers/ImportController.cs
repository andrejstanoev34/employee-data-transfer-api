using EmployeeDataImporterApi.Domain.CustomAttributes;
using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDataImporterApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly ImportService _importService;
    private readonly ExcelExportService _excelExportService;

    public ImportController(ImportService importService, ExcelExportService excelExportService, StatisticsQueries statisticsQueries)
    {
        _importService = importService;
        _excelExportService = excelExportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllImportRuns()
    {
        var allImportRuns = await _importService.GetAllImportRuns();
        return Ok(allImportRuns);
    }

    [HttpGet("{organisationId}")]
    public async Task<IActionResult> GetImportRunById([FromRoute] Guid organisationId)
    {
        try
        {
            var importRunDto = await _importService.GetImportRunById(organisationId);
            return Ok(importRunDto);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [FileImport]
    [HttpPost("{organisationId}")]
    public async Task<IActionResult> ImportData(List<IFormFile>? files, [FromRoute] Guid organisationId)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest("No Files Uploaded");
        }
        
        var importRunDto = await _importService.ImportData(files, organisationId);
        
        return Ok(importRunDto);
    }

    [HttpGet("{importRunId}/report")]
    public async Task<IActionResult> GetImportReport([FromRoute] Guid importRunId)
    {
        try
        {
            var bytes = await _excelExportService.ExportToExcel(importRunId);

            return File(
                bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"ImportRun-{importRunId:N}.xlsx");
        }
        catch (Exception e)
        {
            BadRequest(e.Message);
        }
        return Ok();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetImportStats()
    {
        var importStatsDto = await _importService.GetImportStats();
        return Ok(importStatsDto);
    }
}