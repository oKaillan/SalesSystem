using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;

public class GetEmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
