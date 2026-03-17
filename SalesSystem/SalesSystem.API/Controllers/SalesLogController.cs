using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Entities;

namespace SalesSystem.API.Controllers;

/// <summary>
/// Responsible Controller for Sales Logs
/// </summary>
/// <param name="slogDal"></param>
/// <param name="empDal"></param>
[ApiController]
[Route("admin/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class SalesLogController(DAL<SalesLog> slogDal, DAL<Employee> empDal) : ControllerBase
{
    private readonly DAL<SalesLog> _slogDal = slogDal;
    private readonly DAL<Employee> _empDal = empDal;

    /// <summary>
    /// Returns all Logs in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet]
    public IActionResult GetLogs(int skip = 0, int take = 50)
    {
        var slogCheck = _slogDal.GetAllInRange(skip, take);
        if (slogCheck is null)
        {
            return NoContent();
        }
        return Ok(slogCheck);
    }

    /// <summary>
    /// Returns a log by it's iD
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    /// <response code="204">If the Log was not found</response>
    [HttpGet("{id:guid}")]
    public IActionResult GetLogByiD(Guid id)
    {
        var slogCheck = _slogDal.GetBy(s => s.SaleId == id);
        if (slogCheck is null)
        {
            return NoContent();
        }
        return Ok(slogCheck);
    }

    /// <summary>
    /// Returns a log by Employee iD
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    /// <response code="204">If the Log was not found</response>
    [HttpGet("{employeeId:int}")]
    public IActionResult GetLogByEmployeeiD(int employeeId)
    {
        var getEmployee = _empDal.GetBy(e => e.Id == employeeId);
        if (getEmployee is null)
            return NotFound("Employee not found.");

        var log = _slogDal.GetAllBy(l => l.EmployeeId == employeeId);
        if (log is null)
            return NotFound("Employee hasn't sales.");
        return Ok(log);
    }

    /// <summary>
    /// Returns a log by Year
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    /// <response code="204">If the Log was not found</response>
    [HttpGet("{startYear:int}-{endYear:int}")]
    public IActionResult GetLogByDate(int startYear, int endYear)
    {
        var slogCheck = _slogDal.GetBy(s => s.Time.Year >= startYear && s.Time.Year <= endYear);
        if (slogCheck is null)
        {
            return NoContent();
        }
        return Ok(slogCheck);
    }
 
}
