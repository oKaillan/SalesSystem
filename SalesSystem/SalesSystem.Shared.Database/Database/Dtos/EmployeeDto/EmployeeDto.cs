using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;

public class EmployeeDto
{
    [Required]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "Name can only have letters and spaces.")]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public EmployeeDto(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
