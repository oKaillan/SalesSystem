using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Services;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.ProductCategoryDto;
using SalesSystem.Shared.Database.Database.Dtos.ProductDto;
using SalesSystem.Shared.Database.Entities;
using SalesSystem.Shared.Database.Responses;

namespace SalesSystem.API.Controllers;
/// <summary>
/// Controller Responsibly to delivery Products Categories
/// </summary>
/// <param name="catService"></param>
[ApiController]
[Route("[controller]")]
[Authorize(Roles = Roles.AdminOrEmployee)]
public class ProductCategoryController(ProductCategoryService catService) : ControllerBase
{
    private readonly ProductCategoryService _catService = catService;
    /// <summary>
    /// Returns all Categories in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GetCategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoriesAsync(int skip = 0, int take = 50)
    {
        return (await _catService.GetAllAsync(skip, take)).ToActionResult();
    }

    /// <summary>
    /// Return Category by it's id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetCategoryDto), StatusCodes.Status200OK)]
    [ActionName(nameof(GetCategoryByIdAsync))]
    public async Task<IActionResult> GetCategoryByIdAsync(int id)
    {
        return (await _catService.GetByAsync(id)).ToActionResult();
    }

    /// <summary>
    /// Create a Category at database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="201">If the creation was successful</response>
    [HttpPost]
    public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryDto pCategory)
    {
        var result = await _catService.CreateAsync(pCategory);
        if (result.Status == Enum.ResultStatus.Created)
        {
            return CreatedAtAction(
                nameof(GetCategoryByIdAsync),
                new { id = result.Data!.id },
                result.Data
                );
        }
        return BadRequest();
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
    public async Task<IActionResult> UpdateCategoryAsync(
        [FromBody] CreateCategoryDto pCategory,
        int id)
    {
        return (await _catService.UpdateAsync(pCategory, id)).ToActionResult();
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
        return (await _catService.DeleteAsync(id)).ToActionResult();
    }
}