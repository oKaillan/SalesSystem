using SalesSystem.Entities;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Shared.Database.Database.Dtos.ProductDto;

public class ProductDto
{
    [Required(ErrorMessage = "Name can't be null")]
    public string Name { get; set; }
    [Range(0, 2200, ErrorMessage = "The minimum quantity is 0.")]
    [Required]
    public int Quantity { get; set; }
    [Required]
    [Range(4.00, Double.PositiveInfinity, ErrorMessage = "The minimum price is 4.00")]
    public double Price { get; set; }

    public ProductCategory? Category { get; set; }


    public ProductDto(string name, int quantity, double price, ProductCategory category)
    {
        Name = name;
        Quantity = quantity;
        Price = price;
        Category = category;
    }
}
