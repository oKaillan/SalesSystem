using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SalesSystem.Entities
{
    public class Product
    {
        [Key]
        [Required]
        public int Id { get; private set; }
        [Required(ErrorMessage = "Name can't be null")]
        public string Name { get; private set; }
        [Range(0, 2200, ErrorMessage = "The minimum quantity is 0.")]
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Range(4.00, Double.PositiveInfinity, ErrorMessage = "The minimum price is 4.00")]
        public double Price { get; set; }

        public ProductCategory? Category { get; set; }


        public Product(string name, int quantity, double price, ProductCategory category)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
            Category = category;
        }

        public Product() { }

        public void RemoveStock(int qtd)
        {
            Quantity -= qtd;
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
        public void ChangeProductCategory(ProductCategory category)
        {
            Category = category;
        }

        public override string ToString()
        {
            return "\nProduct Information:\n\n" +
                $"iD: {Id}\n" +
                $"Name: {Name}\n" +
                $"Category: {Category}\n" +
                $"Quantity: {Quantity}\n" +
                $"Price: ${Price.ToString("F2", CultureInfo.InvariantCulture)}\n";
        }
    }
}
