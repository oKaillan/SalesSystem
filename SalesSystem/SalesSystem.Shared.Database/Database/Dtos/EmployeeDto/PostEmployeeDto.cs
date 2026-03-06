using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Shared.Database.Database.Dtos.EmployeeDto
{
    public class PostEmployeeDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "Name can only have letters and spaces.")]
        public string Name { get; private set; }
        [Required]
        [EmailAddress]
        public string Email { get; private set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; private set; }

        public PostEmployeeDto(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
