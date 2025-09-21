using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;

namespace SalesSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SalesLogController : ControllerBase
{
    private readonly DAL<SalesLog> _slogDal;
    private readonly DAL<Employee> _empDal;

    public SalesLogController(DAL<SalesLog> slogDal, DAL<Employee> empDal)
    {
        _slogDal = slogDal;
        _empDal = empDal;
    }

    /// <summary>
    /// Returns all Logs in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet]
    public IActionResult GetLogs()
    {
        var slogCheck = _slogDal.GetAll();
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
        var slogCheck = _slogDal.GetBy(s => s.SaleId.Equals(id));
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
 
}
