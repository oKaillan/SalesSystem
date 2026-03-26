using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Services;
using SalesSystem.Shared.Database.Database.Dtos;
using SalesSystem.Shared.Database.Entities;

namespace SalesSystem.API.Controllers;

/// <summary>
/// Responsible Controller for Sales Logs
/// </summary>
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
    public async Task<IActionResult> GetLogsAsync(int skip = 0, int take = 50)
    {
        return (await _logService.GetAllAsync(skip, take)).ToActionResult();
    }

    /// <summary>
    /// Returns a log by it's iD
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    /// <response code="204">If the Log was not found</response>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetLogByiDAsync(Guid id)
    {
        return (await _logService.GetByIdAsync(id)).ToActionResult();
    }

    /// <summary>
    /// Returns a log by Employee iD
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    /// <response code="204">If the Log was not found</response>
    [HttpGet("{employeeId:int}")]
    public async Task<IActionResult> GetLogByEmployeeiDAsync(int employeeId)
    {
        return (await _logService.GetByEmployeeIdAsync(employeeId)).ToActionResult();
    }

    /// <summary>
    /// Returns a log by Year
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    /// <response code="204">If the Log was not found</response>
    [HttpGet("{startYear:int}-{finalYear:int}")]
    public async Task<IActionResult> GetLogByDateAsync(int startYear, int finalYear)
    {
        return (await _logService.GetByDateAsync(startYear, finalYear)).ToActionResult();
    }

    /// <summary>
    /// Creates SalesLog
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateLogAsync(SalesLogDto dto)
    {
        return (await _logService.CreateAsync(dto)).ToActionResult();
    }
}
