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

    public ImportController(ImportService importService)
    {
        _importService = importService;
    }

    [HttpPost]
    public async Task<IActionResult> ImportData(List<IFormFile>? files)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest("No Files Uploaded");
        }
        
        await _importService.ImportData(files);
        
        return Ok();
        
    }
}