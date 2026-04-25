using SalesSystem.Shared.Database.Enum;

namespace SalesSystem.Shared.Database.Database.Dtos.FilterDto;

public class ProductFilterDto
{

    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public ProductOrderByFilter? OrderBy { get; set; }
    public bool Desc { get; set; } = false;

    public ProductFilterDto() { }

    public ProductFilterDto(string? productName, int? categoryId, ProductOrderByFilter? orderBy, bool desc = false)
    {
        Name = productName;
        CategoryId = categoryId;
        OrderBy = orderBy;
        Desc = desc;
    }
};
