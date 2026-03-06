using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Entities
{
    public class Employee
    {
        [Key]
        [Required]
        public int Id { get; private set; }
        [Required]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "Name can only have letters and spaces.")]
        public string Name { get; private set; }
        [Required]
        [EmailAddress]
        public string Email { get; private set; }
        [PasswordPropertyText]
        public string Password { get; private set; }

        public Employee(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public void ChangeEmployeeName(string name)
        {
            Name = name;
        }
        public void ChangeEmployeeEmail(string email)
        {
            Email = email;
        }

        public void ChangeEmployeePassword(string password)
        {
            Password = password;
        }

        public override string ToString()
        {
            return "\nEmployee Information:\n\n" +
                   $"iD: {Id}\n" +
                   $"Name: {Name}\n" +
                   $"Email: {Email}\n";
        }
    }
}
