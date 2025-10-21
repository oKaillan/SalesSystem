using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalesSystem.Entities
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; private set; }
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }

        public ProductCategory(){ }

        public ProductCategory(string name)
        {
            Name = name;
        }

        public void ChangeProductName(string name)
        {
            Name = name;
        }
    }
}
