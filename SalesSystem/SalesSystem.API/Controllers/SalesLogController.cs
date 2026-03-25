using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Services;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos;
using SalesSystem.Shared.Database.Entities;

namespace SalesSystem.API.Controllers;

/// <summary>
/// Responsible Controller for Sales Logs
/// </summary>
/// <param name="slogDal"></param>
/// <param name="empDal"></param>
/// <param name="prodDal"></param>
/// <param name="logService"></param>
[ApiController]
[Route("admin/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class SalesLogController(SalesLogService logService) : ControllerBase
{
    private readonly SalesLogService _logService = logService;

    /// <summary>
    /// Returns all Logs in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet]
    public IActionResult GetLogs(int skip = 0, int take = 50)
    {
        return _logService.GetAll(skip, take).ToActionResult();
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
        return _logService.GetById(id).ToActionResult();
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
        return _logService.GetByEmployeeId(employeeId).ToActionResult();
    }

    /// <summary>
    /// Returns a log by Year
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    /// <response code="204">If the Log was not found</response>
    [HttpGet("{startYear:int}-{finalYear:int}")]
    public IActionResult GetLogByDate(int startYear, int finalYear)
    {
        return _logService.GetByDate(startYear, finalYear).ToActionResult();
    }

    /// <summary>
    /// Creates SalesLog
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateLog(SalesLogDto dto)
    {
        return _logService.Create(dto).ToActionResult();
    }
}
