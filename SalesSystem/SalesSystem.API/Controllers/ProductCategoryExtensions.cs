using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;

namespace SalesSystem.API.Controllers
{
    public static class ProductCategoryExtensions
    {
        public static void AddEndPointsProductCategory(this WebApplication app)
        {
            //Get all Product Categories
            app.MapGet("/ProductCategory/", ([FromServices] DAL<ProductCategory> pCategoryDAL) =>
            {
                var categories = pCategoryDAL.GetAll();
                return Results.Ok(categories);
            });

            //Creates a new Category
            app.MapPost("/ProductCategory/", ([FromBody] ProductCategory pCategory, [FromServices] DAL<ProductCategory> pCategoryDAL) =>
            {
                var categoryCheck = pCategoryDAL.GetBy(c => c.Name == pCategory.Name); // Checks if Category already exists
                if (categoryCheck is not null)
                {
                    return Results.Conflict("Category already exist.");
                }

                pCategoryDAL.Create(pCategory);

                return Results.Created($"/ProductCategory/{pCategory.Name}", pCategory);
            });


            /* Need to change Category in Database to make this feature
            //Update Category name
            app.MapPut("/ProductCategory/", ([FromBody] ProductCategory pCategory, [FromServices] DAL<ProductCategory> pCategoryDAL, string categoryName) =>
            {
                var getCategory = pCategoryDAL.GetBy(p => p.Name.ToLower() == categoryName.ToLower());
                if (getCategory is null)
                {
                    return Results.Conflict("Category does not exist.");
                }

                getCategory.ChangeProductName(pCategory.Name);
                pCategoryDAL.Update(getCategory);
                return Results.Ok("Product Updated with success.");
            });
            */

            //Delete Category
            app.MapDelete("/ProductCategory/{name}", ([FromServices] DAL<ProductCategory> pCategoryDAL, string name) =>
            {
                var categoryCheck = pCategoryDAL.GetBy(p => p.Name.ToLower() == name.ToLower());
                if (categoryCheck is null)
                {
                    return Results.Conflict("Category does not exist.");
                }

                pCategoryDAL.Delete(categoryCheck);
                return Results.NoContent();
            });


        }
    }
}
