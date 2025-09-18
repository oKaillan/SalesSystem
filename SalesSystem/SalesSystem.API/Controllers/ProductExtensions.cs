using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Requests;
using SalesSystem.Database;
using SalesSystem.Entities;

namespace SalesSystem.API.Controllers
{
    public static class ProductExtensions
    {
        public static void AddEndPointsProduct(this WebApplication app)
        {
            //Get all Products
            app.MapGet("/Products", ([FromServices] DAL<Product> prodDAL) =>
            {
                var getProducts = prodDAL.GetAll();
                if (getProducts is null)
                {
                    Results.NotFound("There's no Products in database.");
                }
                return Results.Ok(getProducts);
            });

            //Get Product by iD
            app.MapGet("/Products/{id}", ([FromServices] DAL<Product> product, int inputiD) =>
            {
                var getProduct = product.GetBy(e => e.Id == inputiD);
                if (getProduct is not null)
                {
                    return Results.Ok(getProduct);
                }
                return Results.NotFound("Product iD not found.");
            });
            //Create a new Product
            app.MapPost("Products/", ([FromBody] ProductRequest productRequest, [FromServices] DAL<Product> prodDAL, [FromServices] DAL<ProductCategory> categoryDAL) =>
            {
                var getProduct = prodDAL.GetBy(e => e.Name.ToLower() == productRequest.name.ToLower());
                if (getProduct is not null)
                {
                    return Results.Conflict("Product with this Name already exists.");
                }

                //Check if Category exists in database
                var categoryCheck = categoryDAL.GetBy(c => c.Name.ToLower() == productRequest.category.Name.ToLower());
                if (categoryCheck is null)
                {
                    return Results.NotFound("Category not found in database.");
                }
                //

                var product = new Product(productRequest.name, productRequest.quantity, productRequest.price, productRequest.category);
                prodDAL.Create(product);
                return Results.Created($"Products/{product.Id}", product);
            });

            //Update Product
            app.MapPut("Products/{id}", ([FromBody] ProductUpdateRequest productUpdated, [FromServices] DAL<Product> prodDAL, [FromServices] DAL<ProductCategory> categoryDAL, int id) =>
            {
                var getProduct = prodDAL.GetBy(e => e.Id.Equals(id));
                if (getProduct is null)
                {
                    return Results.NotFound("Product iD not Found.");
                }

                //Check if Category exists in database
                var categoryCheck = categoryDAL.GetBy(c => c.Name.ToLower() == productUpdated.category.Name.ToLower());
                if (categoryCheck is null)
                {
                    return Results.NotFound("Category not found in database.");
                }
                //
                getProduct.ChangeProductName(productUpdated.name);
                getProduct.ChangeProductPrice(productUpdated.price);
                getProduct.ChangeProductCategory(productUpdated.category);
                prodDAL.Update(getProduct);
                return Results.Ok("Product updated with success.");
            });

            //Delete Product
            app.MapDelete("Product/{id}", ([FromServices] DAL<Product> prodDAL, int id) =>
            {
                var getProduct = prodDAL.GetBy(e => e.Id.Equals(id));
                if (getProduct is null)
                {
                    return Results.NotFound("Product iD not Found.");
                }
                prodDAL.Delete(getProduct);
                return Results.NoContent();
            });
        }
    }
}
