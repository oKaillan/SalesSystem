using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Entities;

namespace SalesSystem.API.Controllers;
/// <summary>
/// Controller Responsibly to delivery Products Categories
/// </summary>
/// <param name="pCategoryDal"></param>
[ApiController]
[Route("[controller]")]
[Authorize(Roles = Roles.AdminOrEmployee)]
public class ProductCategoryController(DAL<ProductCategory> pCategoryDal) : ControllerBase
{
    private readonly DAL<ProductCategory> _pCategoryDal = pCategoryDal;

    /// <summary>
    /// Returns all Categories in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet]
    public async Task<IActionResult> GetCategoriesAsync(int skip = 0, int take = 50)
    {
        var categories = await _pCategoryDal.GetAllPagedWithSelectorAsync(skip, take, c => new { c.Id, c.Name } );
        return Ok(categories);
    }

    /// <summary>
    /// Create a Category at database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="201">If the creation was successful</response>
    [HttpPost]
    public async Task<IActionResult> CreateCategoryAsync([FromBody] ProductCategory pCategory)
    {
        var categoryCheck = _pCategoryDal.GetByAsync(c => c.Name == pCategory.Name); // Checks if Category already exists
        if (categoryCheck is not null)
        {
            return Conflict("Category already exist.");
        }

        await _pCategoryDal.CreateAsync(pCategory);

        return Created($"/ProductCategory/{pCategory.Id}", pCategory);
    }




    /// <summary>
    /// Update a Category at Database
    /// </summary>
    /// <param name="pCategory">Object with the neccessary fields</param>
    /// <param name="id">Category iD</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] ProductCategory pCategory, 
        int id)
        {
            var getCategory = await _pCategoryDal.GetByAsync(c => c.Id == id);
            if (getCategory is null)
            {
                return NotFound("Category does not exist.");
            }

            getCategory.ChangeCategoryName(pCategory.Name);
            _pCategoryDal.Update(getCategory);
            return NoContent();
        }


    /// <summary>
    /// Delete a Category at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the delete was successful</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("admin/{id}")]
    public async Task<IActionResult> DeleteCategoryAsync(int id)
    {
        var categoryCheck = await _pCategoryDal.GetByAsync(p => p.Id == id);
        if (categoryCheck is null)
        {
            return NotFound("Category does not exist.");
        }

        _pCategoryDal.Delete(categoryCheck);
        return NoContent();
    }

}