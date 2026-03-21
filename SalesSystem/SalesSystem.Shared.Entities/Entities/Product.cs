using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace SalesSystem.Entities
{
    public class Product
    {
        [Key]
        [Required]
        public int Id { get; private set; }
        [Required(ErrorMessage = "Name can't be null")]
        public string Name { get; private set; } = null!;
        [Range(0, 2200, ErrorMessage = "The minimum quantity is 0.")]
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Range(4.00, Double.PositiveInfinity, ErrorMessage = "The minimum price is 4.00")]
        public double Price { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductCategory> Categories { get; set; } = new List<ProductCategory>();



        public Product(string name, int quantity, double price, ProductCategory category)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
            Categories.Add(category);
        }

        public Product()
        {
        }

        public (bool Success, string? Error) TryRemoveStock(int qtd)
        {
            if (qtd <= 0)
                return (false, "Quantity can't be equal or less than 0");

            if (qtd > Quantity)
                return (false, "Specified quantity bigger than Product Quantity");

            Quantity -= qtd;
            return (true, null);
        }

        public double GetTotalStockPrice()
        {
            return Price * Quantity;
        }
        public double GetTotalPrice(int quantity)
        {
            return quantity * Price;
        }

        public void ChangeProductName(string name)
        {
            Name = name;
        }
        public void ChangeProductPrice(double price)
        {
            Price = price;
        }

        public void ChangeProductCategories(ICollection<ProductCategory> categories)
        {
            Categories = categories;
        }

        public override string ToString()
        {
            return "\nProduct Information:\n\n" +
                $"iD: {Id}\n" +
                $"Name: {Name}\n" +
                $"Category: {Categories}\n" +
                $"Quantity: {Quantity}\n" +
                $"Price: ${Price.ToString("F2", CultureInfo.InvariantCulture)}\n";
        }
    }
}
