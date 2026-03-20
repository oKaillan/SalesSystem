using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;
using Microsoft.AspNetCore.Identity;
using SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;
using Microsoft.AspNetCore.Authorization;
using SalesSystem.Shared.Database.Entities;
using BC = BCrypt.Net.BCrypt;

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
public class EmployeeController(IMapper mapper, DAL<Employee> empDAL, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly DAL<Employee> _empDAL = empDAL;
    private readonly UserManager<ApplicationUser> _userManager = userManager;


    /// <summary>
    /// Returns all Employees in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    //[HttpGet]
    //public IActionResult GetEmployees(int skip = 0, int take = 50)
    //{
    //    var getEmployees = _empDAL.GetAllPaged(skip, take);
    //    if (getEmployees is null)
    //        return NotFound("There's no Employees in database.");

    //    var employees = _mapper.Map<List<GetEmployeeDto>>(getEmployees);

    //    return Ok(employees);
    //}

    /// <summary>
    /// Returns an Employee by it's iD
    /// </summary>
    /// <param name="id">Object with the neccessary fields</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Employee was found</response>
    [HttpGet("{id}")]
    public IActionResult GetEmployeeById(int id)
    {
        var getEmployee = _empDAL.GetBy(e => e.Id == id);
        if (getEmployee is null)
            return NotFound("Employee iD not found.");

        var empMapper = _mapper.Map<GetEmployeeDto>(getEmployee);
        return Ok(empMapper);
    }

    /// <summary>
    /// Create an Employee at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="201">If the creation was successful</response>
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeAsync([FromBody] EmployeeDto employeeDto)
    {
        var getEmployee = _empDAL.GetBy(e => e.Email == employeeDto.Email);
        if (getEmployee is not null)
            return Conflict("Employee with this email already exists.");

        var user = new ApplicationUser
        {
            UserName = employeeDto.Email,
            Email = employeeDto.Email
        };

        var result = await _userManager.CreateAsync(user, employeeDto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, Roles.Employee);

        var passwordCrypt = BC.HashPassword(employeeDto.Password);

        var employee = _mapper.Map<Employee>(employeeDto);

        //Change the Password to encrypted one
        employee.ChangeEmployeePassword(passwordCrypt);

        _empDAL.Create(employee);
        return CreatedAtAction(nameof(GetEmployeeById),
        new { id = employee.Id },
        employee);
    }

    /// <summary>
    /// Update an Employee at Database
    /// </summary>
    /// <param name="employeeUpdated">Object with the neccessary fields</param>
    /// <param name="id">Employee iD</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeAsync(int id, [FromBody] EmployeeDto employeeUpdated)
    {
        var getEmployee = _empDAL.GetBy(e => e.Id.Equals(id));
        var user = await _userManager.FindByEmailAsync(getEmployee?.Email!);

        if (getEmployee is null || user is null)
            return NotFound("Employee not Found.");

        //Changes Employee in Database
        getEmployee.ChangeEmployeeName(employeeUpdated.Name);
        getEmployee.ChangeEmployeeEmail(employeeUpdated.Email);

        //Changes Employee User in IdentityDB
        user.UserName = employeeUpdated.Email;
        user.Email = employeeUpdated.Email;
        await _userManager.UpdateAsync(user);
        //Change Password
        if (!string.IsNullOrEmpty(employeeUpdated.Password))
        {
            //Change Password in Employee Database
            var passwordCrypt = BC.HashPassword(employeeUpdated.Password);
            getEmployee.ChangeEmployeePassword(passwordCrypt);

            //Change Password in IdentityDB
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var tryPasswordChange = await _userManager.ResetPasswordAsync(
                user, 
                token, 
                employeeUpdated.Password);

            if (!tryPasswordChange.Succeeded)
                return BadRequest(tryPasswordChange.Errors);
        }

        _empDAL.Update(getEmployee);
        return NoContent();
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
        var getEmployee = _empDAL.GetBy(e => e.Id.Equals(id));
        var user = await _userManager.FindByEmailAsync(getEmployee?.Email!);
        if (getEmployee is null || user is null) 
            return NotFound("Employee not Found.");

        var empToUpdate = _mapper.Map<PatchEmployeeDto>(getEmployee);
        patch.ApplyTo(empToUpdate, ModelState);
        if (!TryValidateModel(empToUpdate)) return ValidationProblem(ModelState);

        _mapper.Map(empToUpdate, getEmployee);

        // Updates Identity if Email's changed
        if (user.Email != empToUpdate.Email)
        {
            user.Email = empToUpdate.Email;
            user.UserName = empToUpdate.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
        }

        // Updates Identity if Password's changed
        if (!string.IsNullOrEmpty(empToUpdate.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var tryPasswordChange = await _userManager.ResetPasswordAsync(
                user,
                token,
                empToUpdate.Password);

            if (!tryPasswordChange.Succeeded)
                return BadRequest(tryPasswordChange.Errors);

            var passwordCrypt = BC.HashPassword(empToUpdate.Password);
            getEmployee.ChangeEmployeePassword(passwordCrypt);
        }


        _empDAL.Update(getEmployee);
        return NoContent();
    }

    /// <summary>
    /// Delete an Employee at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the delete was successful</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeAsync(int id)
    {
        var getEmployee = _empDAL.GetBy(e => e.Id.Equals(id));
        var user = await _userManager.FindByEmailAsync(getEmployee?.Email!);
        if (getEmployee is null || user is null)
        {
            return NotFound("Employee not Found.");
        }
        _empDAL.Delete(getEmployee);
        await _userManager.DeleteAsync(user);
        return NoContent();
    }
}
