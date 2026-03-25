using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;
using Microsoft.AspNetCore.Identity;
using SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;
using Microsoft.AspNetCore.Authorization;
using SalesSystem.Shared.Database.Entities;
using SalesSystem.API.Services;

namespace SalesSystem.API.Controllers;
/// <summary>
/// Controller Responsibly to delivery Employees
/// </summary>
/// <param name="mapper"></param>
/// <param name="empDAL"></param>
/// <param name="userManager"></param>
[ApiController]
[Route("admin/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class EmployeeController(EmployeeService empService, IMapper mapper, DAL<Employee> empDAL, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly EmployeeService _empService = empService;
    private readonly IMapper _mapper = mapper;
    private readonly DAL<Employee> _empDAL = empDAL;
    private readonly Func<Employee, GetEmployeeDto> getEmployeeDto = e => new GetEmployeeDto(
        e.Id,
        e.Name,
        e.Email
        );
    private readonly UserManager<ApplicationUser> _userManager = userManager;


    /// <summary>
    /// Returns all Employees in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    /// <response code="204">If there are no content</response>
    [HttpGet]
    public IActionResult GetEmployees(int skip = 0, int take = 50)
    {
        return _empService.GetAll(skip, take).ToActionResult();
    }

    /// <summary>
    /// Returns an Employee by it's iD
    /// </summary>
    /// <param name="id">Object with the neccessary fields</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Employee was found</response>
    /// <response code="404">If the Employee was not found</response>
    [HttpGet("{id}")]
    public IActionResult GetEmployeeById(int id)
    {
        return _empService.GetById(id).ToActionResult();
    }

    /// <summary>
    /// Create an Employee at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="201">If the creation was successful</response>
    /// <response code="409">If Employee email is already in use</response>
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeAsync([FromBody] EmployeeDto employeeDto)
    {
        var result = await _empService.CreateAsync(employeeDto);
        if (result.Status == Enum.ResultStatus.Created)
        {
            return CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = result.Data!.iD },
                result.Data
                );
        }
        return result.ToActionResult();
    }

    /// <summary>
    /// Update an Employee at Database
    /// </summary>
    /// <param name="employeeUpdated">Object with the neccessary fields</param>
    /// <param name="id">Employee iD</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    /// <response code="404">If Employee was not found</response>
    /// <response code="400">If Email, Name or Password is not valid</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeAsync(int id, [FromBody] EmployeeDto employeeUpdated)
    {
        return (await _empService.UpdateAsync(id, employeeUpdated)).ToActionResult();
    }

    /// <summary>
    /// Update an Employee field at Database
    /// </summary>
    /// <param name="patch">Object with the neccessary fields</param>
    /// <param name="id">Employee iD</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    [HttpPatch]
    public async Task<IActionResult> PatchEmployeeAsync(int id,
        JsonPatchDocument<PatchEmployeeDto> patch)
    {
        var getEmployee = _empService.GetById(id);

        if (getEmployee.Status == Enum.ResultStatus.NotFound)
            return getEmployee.ToActionResult();

        var getDto = getEmployee.Data;
        var patchDto = _mapper.Map<PatchEmployeeDto>(getDto);
        patch.ApplyTo(patchDto, ModelState);
        if (!TryValidateModel(patchDto))
            return ValidationProblem(ModelState);

        var result = await _empService.PatchAsync(id, patchDto);

        return result.ToActionResult();
    }

    /// <summary>
    /// Changes Employee Password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    /// <response code="404">If Employee was not found</response>
    /// <response code="400">If Password is null or empty</response>
    [HttpPatch("{id}/password")]
    public async Task<IActionResult> ChangeEmployeePassword(int id, [FromBody] ChangePasswordDto dto)
    {
        return (await _empService.ChangePassword(id, dto)).ToActionResult();
    }

    /// <summary>
    /// Delete an Employee at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the delete was successful</response>
    /// <response code="404">If Employee was not found</response>
    /// <response code="400">If something went wrong trying to delete Employee</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeAsync(int id)
    {
        return (await _empService.Delete(id)).ToActionResult();
    }
}
