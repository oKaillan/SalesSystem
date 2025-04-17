using System.Globalization;

namespace SalesSystem.Entities
{
    internal class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public double Price { get; private set; }

        public string Category { get; private set; }


        public Product(string name, int quantity, double price, ProductCategory category)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
            Category = category.Name;
        }

        public Product()
        {
        }

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
