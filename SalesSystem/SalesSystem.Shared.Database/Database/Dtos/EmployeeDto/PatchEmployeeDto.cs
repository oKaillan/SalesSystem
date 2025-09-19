using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
