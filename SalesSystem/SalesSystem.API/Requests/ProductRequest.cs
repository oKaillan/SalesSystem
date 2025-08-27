using SalesSystem.Entities;

namespace SalesSystem.API.Requests
{
    public record ProductRequest(string name, int quantity, double price, ProductCategory category);
    public record ProductUpdateRequest(string name, double price, ProductCategory category);
}
