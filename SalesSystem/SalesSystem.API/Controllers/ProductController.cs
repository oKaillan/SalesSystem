using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Enum;
using SalesSystem.API.Services;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos;
using SalesSystem.Shared.Database.Database.Dtos.FilterDto;
using SalesSystem.Shared.Database.Database.Dtos.ProductDto;
using SalesSystem.Shared.Database.Entities;
using SalesSystem.Shared.Database.Enum;
using SalesSystem.Shared.Database.Responses;
using System.Linq.Expressions;

namespace SalesSystem.API.Controllers;
/// <summary>
/// Controller Responsible to delivery Products
/// </summary>
/// <param name="prodService"></param>
[ApiController]
[Route("[controller]")]
[Authorize(Roles = Roles.AdminOrEmployee)]
public class ProductController(ProductService prodService) : ControllerBase
{
    private readonly ProductService _prodService = prodService;
    /// <summary>
    /// Returns all Products in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GetProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsAsync(
        string? name, 
        int? categoryId,
        ProductOrderByFilter? orderBy,
        bool descending = false,
        int skip = 0, 
        int take = 50)
    {
        var filter = new ProductFilterDto(name, categoryId, orderBy, descending);
        return (await _prodService.GetAllAsync(skip, take, filter)).ToActionResult();
    }

    /// <summary>
    /// Returns a Product by it's iD
    /// </summary>
    /// <param name="id">Object with the neccessary fields</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Product was found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetProductDto), StatusCodes.Status200OK)]
    [ActionName(nameof(GetProductByIdAsync))] // Necessary to return product when created.
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        return (await _prodService.GetByIdAsync(id)).ToActionResult();
    }

    /// <summary>
    /// Create a Product at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="201">If the creation was successful</response>
    [HttpPost]
    public async Task<IActionResult> PostProductAsync([FromBody] ProductDto productDto)
    {
        var product = await _prodService.CreateAsync(productDto);
        if (product.Status == Enum.ResultStatus.Created)
        {
            return CreatedAtAction(
                nameof(GetProductByIdAsync),
                new { id = product.Data!.iD },
                product.Data);
        }
        return product.ToActionResult();
    }


    /// <summary>
    /// Update a Product at Database
    /// </summary>
    /// <param name="productDto">Object with the neccessary fields</param>
    /// <param name="id">Object iD</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpPut("admin/{id}")]
    public async Task<IActionResult> UpdateProductAsync([FromBody] ProductDto productDto,
        int id)
    {
        return (await _prodService.UpdateAsync(productDto, id)).ToActionResult();
    }

    /// <summary>
    /// Update a Product field at Database
    /// </summary>
    /// <remarks>If Category was not selected or was invalid, it backs to the previous value.<br/>
    /// To update categories, use the <c>/categories</c> path with a list of categories Ids.
    /// </remarks>
    /// <param name="patch">Object with the neccessary fields</param>
    /// <param name="id">Product iD</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpPatch("admin")]
    public async Task<IActionResult> PatchProductAsync(int id,
        JsonPatchDocument<PatchProductDto> patch)
    {
        var result = await _prodService.PatchAsync(id, patch);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return ValidationProblem(ModelState);
        }
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a Product at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the delete was successful</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("admin/{id}")]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        return (await _prodService.DeleteAsync(id)).ToActionResult();
    }

}