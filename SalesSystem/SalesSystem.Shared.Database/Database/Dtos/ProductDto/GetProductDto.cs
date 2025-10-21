namespace SalesSystem.Shared.Database.Database.Dtos.ProductDto
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public List<string> Categories { get; set; } 
    }
}
