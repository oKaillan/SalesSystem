using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Entities
{
    internal class ProductCategory
    {
        [Key]
        public string Name { get; private set; }

        public ProductCategory(string name)
        {
            Name = name;
        }

        public ProductCategory() { }
    }
}
