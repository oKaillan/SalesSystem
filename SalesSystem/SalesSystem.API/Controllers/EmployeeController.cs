using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;
using Microsoft.AspNetCore.Identity;
using SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;
using Microsoft.AspNetCore.Authorization;
using SalesSystem.Shared.Database.Entities;

namespace SalesSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = Roles.Admin)]
public class EmployeeController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly DAL<Employee> _empDAL;

    public EmployeeController(IMapper mapper, DAL<Employee> empDAL)
    {
        _mapper = mapper;
        _empDAL = empDAL;
    }


    /// <summary>
    /// Returns all Employees in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet]
    public IActionResult GetEmployees(int skip = 0, int take = 50)
    {
        var getEmployees = _empDAL.GetAllInRange(skip, take);
        if (getEmployees is null)
            return NotFound("There's no Employees in database.");

        return Ok(getEmployees);
    }

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

        var empMapper = _mapper.Map<EmployeeDto>(getEmployee);
        return Ok(empMapper);
    }

    /// <summary>
    /// Create an Employee at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="201">If the creation was successful</response>
    [HttpPost]
    public IActionResult CreateEmployee([FromBody] EmployeeDto employeeDto)
    {
        var getEmployee = _empDAL.GetBy(e => e.Email == employeeDto.Email);
        if (getEmployee is not null)
            return Conflict("Employee with this email already exists.");

        var employee = _mapper.Map<Employee>(employeeDto);

        _empDAL.Create(employee);
        return CreatedAtAction(nameof(GetEmployeeById),
        new { id = employee.Id },
        employee);
    }

    /// <summary>
    /// Update an Employee at Database
    /// </summary>
    /// <param name="employeeUpdated">Object with the neccessary fields</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    [HttpPut("{id}")]
    public IActionResult UpdateEmployee(int id, [FromBody] EmployeeDto employeeUpdated)
    {
        var getEmployee = _empDAL.GetBy(e => e.Id.Equals(id));

        if (getEmployee is null)
           return NotFound("Employee iD not Found.");

        getEmployee.ChangeEmployeeName(employeeUpdated.Name);
        getEmployee.ChangeEmployeeEmail(employeeUpdated.Email);
        _empDAL.Update(getEmployee);
        return NoContent();
    }

    /// <summary>
    /// Update an Employee field at Database
    /// </summary>
    /// <param name="patch">Object with the neccessary fields</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    [HttpPatch]
    public IActionResult PatchEmployee(int id,
        JsonPatchDocument<PatchEmployeeDto> patch)
    {
        var getEmployee = _empDAL.GetBy(e => e.Id.Equals(id));
        if (getEmployee is null) return NotFound("Employee iD not Found.");

        var empToUpdate = _mapper.Map<PatchEmployeeDto>(getEmployee);
        patch.ApplyTo(empToUpdate, ModelState);
        if (!TryValidateModel(empToUpdate)) return ValidationProblem(ModelState);

        _mapper.Map(empToUpdate, getEmployee);
        _empDAL.Update(getEmployee);
        return NoContent();
    }

    /// <summary>
    /// Delete an Employee at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the delete was successful</response>
    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        var getEmployee = _empDAL.GetBy(e => e.Id.Equals(id));
        if (getEmployee is null)
        {
            return NotFound("Employee iD not Found.");
        }
        _empDAL.Delete(getEmployee);
        return NoContent();
    }
}
