using EmployeeDataImporterApi.Domain.Dto;
using EmployeeDataImporterApi.Services.Implementation;
using EmployeeDataImporterApi.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDataImporterApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly ImportService _importService;
    private readonly ExcelExportService _excelExportService;

    public ImportController(ImportService importService, ExcelExportService excelExportService)
    {
        _importService = importService;
        _excelExportService = excelExportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllImportRuns()
    {
        var res = await _importService.GetAllImportRuns();
        return Ok(res);
    }

    [HttpGet("{organisationId}")]
    public async Task<IActionResult> GetImportRun([FromRoute] Guid organisationId)
    {
        var res = await _importService.GetImportRunById(organisationId);
        return Ok(res);
    }
    
    [HttpPost("{organisationId}")]
    public async Task<IActionResult> ImportData(List<IFormFile>? files, [FromRoute] Guid organisationId)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest("No Files Uploaded");
        }
        
        var res = await _importService.ImportData(files, organisationId);
        
        return Ok(res);
        
    }

    [HttpGet("{importRunId}/report")]
    public async Task<IActionResult> GetImportReport([FromRoute] Guid importRunId)
    {
        //treba se fane exception
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
}