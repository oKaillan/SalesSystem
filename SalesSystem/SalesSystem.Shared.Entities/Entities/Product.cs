using SalesSystem.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace SalesSystem.Entities;

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

    public EntitiesResult TryRemoveStock(int qtd)
    {
        if (qtd <= 0)
            return (new EntitiesResult(false, "Quantity can't be equal or less than 0"));

        if (qtd > Quantity)
            return (new EntitiesResult(false, "Specified quantity is bigger than Product Quantity"));

        Quantity -= qtd;
        return (new EntitiesResult(true, null));
    }

    public double GetTotalStockPrice()
    {
        return Price * Quantity;
    }
    public double GetTotalPrice(int quantity)
    {
        return quantity * Price;
    }

    public EntitiesResult TryChangeProductName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return (new EntitiesResult(false, "Name property can't be null."));
        if (name == Name)
            return (new EntitiesResult(false, "Product Name is the same of input"));

        Name = name;
        return (new EntitiesResult(true, null));
    }
    public EntitiesResult TryChangeProductPrice(double price)
    {
        if (price <= 0)
            return (new EntitiesResult(false, "Price can't be less or equal 0!"));

        if (price == Price)
            return (new EntitiesResult(false, "Product Price is the same of input"));

        Price = price;
        return (new EntitiesResult(true, null));
    }

    public (bool Success, string? Error) TryChangeProductCategories(ICollection<ProductCategory> categories)
    {
        if (categories is null || categories.Count == 0)
            return (false, "Categories can't be null or empty");

        Categories.Clear();

        foreach (var category in categories)
            Categories.Add(category);

        return (true, null);
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
