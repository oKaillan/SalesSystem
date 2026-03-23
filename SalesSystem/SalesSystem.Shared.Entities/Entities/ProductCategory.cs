using SalesSystem.Shared.Entities;
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

        public EntitiesResult ChangeCategoryName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return (new EntitiesResult(false, "Name can't be null or empty"));
            
            if (name == Name)
                return (new EntitiesResult(false, "Name can't be the same"));

            Name = name;
            return (new EntitiesResult(true, null));
        }
    }
}
