using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Shared.Database.Database.Dtos.EmployeeDto
{
    public class PatchEmployeeDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "Name can only have letters and spaces.")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

    }
}
