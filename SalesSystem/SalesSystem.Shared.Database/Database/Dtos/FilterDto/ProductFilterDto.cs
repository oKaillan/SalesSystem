namespace SalesSystem.Shared.Database.Database.Dtos.FilterDto;

public class ProductFilterDto
{

    public string? Name { get; set; }
    public int? CategoryId { get; set; }

    public ProductFilterDto() { }

    public ProductFilterDto(string? productName, int? categoryId)
    {
        Name = productName;
        CategoryId = categoryId;
    }
};
