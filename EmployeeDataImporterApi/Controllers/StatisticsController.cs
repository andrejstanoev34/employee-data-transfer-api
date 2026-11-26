using DocumentFormat.OpenXml.Office2010.ExcelAc;
using EmployeeDataImporterApi.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDataImporterApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly StatisticsQueries _statisticsQueries;

    public StatisticsController(StatisticsQueries statisticsQueries)
    {
        _statisticsQueries = statisticsQueries;
    }

    [HttpGet("topEarners")]
    public async Task<IActionResult> GetTopEarnersForDepartments()
    {
        var departmentTopErarners = await _statisticsQueries.TopEarners();
        
        return Ok(departmentTopErarners);
    }

    [HttpGet("emptySpots")]
    public async Task<IActionResult> GetEmptySpotsForDepartments()
    {
        var departmentCounts = await _statisticsQueries.EmptySpots();
        return Ok(departmentCounts);
    }

    [HttpGet("employeeWithSkills")]
    public async Task<IActionResult> GetEmployeeWithSkills()
    {
        var listOfSkills = new List<Guid>()
            { Guid.Parse("a1000001-0000-4000-8000-000000000001"), Guid.Parse("a1000001-0000-4000-8000-000000000011") };
        var employeeWithSkills = await _statisticsQueries.EmployeesWithSkills(listOfSkills);
        if (employeeWithSkills.Count == 0)
        {
            return NoContent();
        }
        
        return Ok(employeeWithSkills);
    }
    
    [HttpGet("tenureCohorts")]
    public async Task<IActionResult> GetTenureCohortsForDepartments()
    {
        var tenureCohorts = await _statisticsQueries.TenureCohorts();
        return Ok(tenureCohorts);
    }

    [HttpGet("runDifference/{organisationId}")]
    public async Task<IActionResult> GetRunDifference([FromRoute]Guid organisationId, [FromQuery]Guid run1, [FromQuery]Guid run2)
    {
        if (run1 == run2)
        {
            return BadRequest("Please pass different import runs");
        }
        
        var differenceDto = await _statisticsQueries.RunDiff(organisationId, run1, run2);
        return Ok(differenceDto);
    }
    
}