using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;

namespace SalesSystem.API.Extensions
{
    public static class EmployeeExtensions
    {
        public static void AddEndPointsEmployee(this WebApplication app)
        {
            //Get all Employees
            app.MapGet("/Employees", ([FromServices] DAL<Employee> empDAL) =>
            {
                var getEmployees = empDAL.GetAll();
                if (getEmployees is null)
                {
                    Results.NotFound("There's no Employees in database.");
                }
                return Results.Ok(getEmployees
                    );
            });

            //Get Employee by iD
            app.MapGet("/Employees/{id}", ([FromServices] DAL<Employee> employee, int inputiD) =>
            {
                var getEmployee = employee.GetBy(e => e.Id == inputiD);
                if (getEmployee is not null)
                {
                    return Results.Ok(getEmployee);
                }
                return Results.NotFound("Employee iD not found.");
            });

            //Create a new Employee
            app.MapPost("Employees/", ([FromBody] Employee employee, [FromServices] DAL<Employee> empDAL) =>
            {
                var getEmployee = empDAL.GetBy(e => e.Email == employee.Email);
                if (getEmployee is not null)
                {
                    return Results.Conflict("Employee with this email already exists.");
                }

                empDAL.Create(employee);
                return Results.Created($"Employees/{employee.Id}", employee);
            });

            //Update Employee
            app.MapPut("Employees/{id}", ([FromBody] Employee employeeUpdated, [FromServices] DAL<Employee> empDAL, int id) =>
            {
                var getEmployee = empDAL.GetBy(e => e.Id.Equals(id));
                if (getEmployee is null)
                {
                    return Results.NotFound("Employee iD not Found.");
                }
                getEmployee.ChangeEmployeeName(employeeUpdated.Name);
                getEmployee.ChangeEmployeeEmail(employeeUpdated.Email);
                empDAL.Update(getEmployee);
                return Results.Ok("Employee updated with success.");
            });

            //Delete Employee
            app.MapDelete("Employee/{id}", ([FromServices] DAL<Employee> empDAL, int id) =>
            {
                var getEmployee = empDAL.GetBy(e => e.Id.Equals(id));
                if (getEmployee is null)
                {
                    return Results.NotFound("Employee iD not Found.");
                }
                empDAL.Delete(getEmployee);
                return Results.NoContent();
            });
        }
    }
}
