namespace SalesSystem.Shared.Database.Database.Dtos.ProductDto;

public record GetProductDto(int iD, string name, int quantity, double price, IEnumerable<CategoryDto>? categories);