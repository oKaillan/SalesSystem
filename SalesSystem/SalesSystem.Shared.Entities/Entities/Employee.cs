using SalesSystem.Shared.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SalesSystem.Entities
{
    public class Employee(string name, string email)
    {
        [Key]
        [Required]
        public int Id { get; private set; }
        [Required]
        [RegularExpression(nameVerifierRegex, ErrorMessage = "Name can only have letters and spaces.")]
        public string Name { get; private set; } = name;
        [Required]
        [EmailAddress]
        public string Email { get; private set; } = email;

        const string nameVerifierRegex = @"^[a-zA-ZÀ-ÿ\s]+$";

        public EntitiesResult TryChangeEmployeeName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return (new EntitiesResult(false, "Name can't be null or empty"));

            if (!Regex.IsMatch(name, nameVerifierRegex))
                return (new EntitiesResult(false, "Name can only have letters and spaces."));

            Name = name;
            return (new EntitiesResult(true, null));
        }
        public EntitiesResult TryChangeEmployeeEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return (new EntitiesResult(false, "Email can't be null"));

            var validator = new EmailAddressAttribute();
            if (!validator.IsValid(email))
                return (new EntitiesResult(false, "This email is not valid!"));

            if (email == Email)
                return (new EntitiesResult(false, "Email can't be the same"));

            Email = email;
            return (new EntitiesResult(true, null));
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
