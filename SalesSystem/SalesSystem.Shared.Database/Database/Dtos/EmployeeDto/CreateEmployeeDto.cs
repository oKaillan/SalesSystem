using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;

internal class CreateEmployeeDto
{
    [Required]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "Name can only have letters and spaces.")]
    public string Name { get; private set; }
    [Required]
    [EmailAddress]
    public string Email { get; private set; }
}
