namespace SalesSystem.Shared.Database.Database.Dtos.ProductDto;

public class GetProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public double Price { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<CategoryDto> Categories { get; set; } = null!;
}
