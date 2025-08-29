using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Entities
{
    public class ProductCategory
    {
        [Key]
        public string Name { get; private set; }

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
