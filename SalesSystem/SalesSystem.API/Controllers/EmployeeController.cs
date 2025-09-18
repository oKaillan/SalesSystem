using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;

namespace SalesSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{

    /// <summary>
    /// Returns all Employees in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet]
    public IActionResult GetEmployees([FromServices] DAL<Employee> empDAL)
    {
        var getEmployees = empDAL.GetAll();
        if (getEmployees is null)
        {
            return NotFound("There's no Employees in database.");
        }
        return Ok(getEmployees);
    }

    /// <summary>
    /// Returns an Employee by it's iD
    /// </summary>
    /// <param name="id">Object with the neccessary fields</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Employee was found</response>
    [HttpGet("{id}")]
    public IActionResult GetEmployeeById([FromServices] DAL<Employee> employee, int id)
    {
        var getEmployee = employee.GetBy(e => e.Id == id);
        if (getEmployee is not null)
        {
            return Ok(getEmployee);
        }
        return NotFound("Employee iD not found.");
    }

    /// <summary>
    /// Create an Employee at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="201">If the creation was successful</response>
    [HttpPost]
    public IActionResult CreateEmployee([FromBody] Employee employee, [FromServices] DAL<Employee> empDAL)
    {
        var getEmployee = empDAL.GetBy(e => e.Email == employee.Email);
        if (getEmployee is not null)
        {
            return Conflict("Employee with this email already exists.");
        }

        empDAL.Create(employee);
        return CreatedAtAction(nameof(GetEmployeeById),

        new { id = employee.Id },
        employee);
    }


    //Update Employee
    /// <summary>
    /// Update an Employee at Database
    /// </summary>
    /// <param name="employeeUpdated">Object with the neccessary fields</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    [HttpPut("{id}")]
    public IActionResult UpdateEmployee([FromBody] Employee employeeUpdated,
            [FromServices] DAL<Employee> empDAL, int id)
    {
        var getEmployee = empDAL.GetBy(e => e.Id.Equals(id));
        if (getEmployee is null)
        {
            return NotFound("Employee iD not Found.");
        }
        getEmployee.ChangeEmployeeName(employeeUpdated.Name);
        getEmployee.ChangeEmployeeEmail(employeeUpdated.Email);
        empDAL.Update(getEmployee);
        return NoContent();
    }
    /*
    [HttpPatch]
    public IActionResult PatchEmployee([FromBody] Employee employeeUpdated,
            [FromServices] DAL<Employee> empDAL, int id)
    {
        var getEmployee = empDAL.GetBy(e => e.Id.Equals(id));
        if (getEmployee is null)
        {
            return NotFound("Employee iD not Found.");
        }
        getEmployee.ChangeEmployeeName(employeeUpdated.Name);
        getEmployee.ChangeEmployeeEmail(employeeUpdated.Email);
        empDAL.Update(getEmployee);
        return NoContent();
    }
    */

    /// <summary>
    /// Delete an Employee at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the delete was successful</response>
    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee([FromServices] DAL<Employee> empDAL, int id)
    {
        var getEmployee = empDAL.GetBy(e => e.Id.Equals(id));
        if (getEmployee is null)
        {
            return NotFound("Employee iD not Found.");
        }
        empDAL.Delete(getEmployee);
        return NoContent();
    }
}
