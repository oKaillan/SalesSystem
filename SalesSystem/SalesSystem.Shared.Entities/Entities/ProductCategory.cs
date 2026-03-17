using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalesSystem.Entities
{
    public class ProductCategory(string name)
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; } = name;
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>(); 

        public void ChangeCategoryName(string name)
        {
            Name = name;
        }
    }
}
