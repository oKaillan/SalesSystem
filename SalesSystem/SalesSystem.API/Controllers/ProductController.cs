using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.ProductDto;

namespace SalesSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly DAL<Product> _prodDAL;

    public ProductController(IMapper mapper, DAL<Product> prodDAL)
    {
        _mapper = mapper;
        _prodDAL = prodDAL;
    }

    /// <summary>
    /// Returns all Products in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet]
    public IActionResult GetProducts()
    {
        var getProducts = _prodDAL.GetProductsWithInclude();
        if (getProducts is null)
        {
            return NotFound("There's no Products in database.");
        }
        return Ok(getProducts);
    }

    /// <summary>
    /// Returns a Product by it's iD
    /// </summary>
    /// <param name="id">Object with the neccessary fields</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Product was found</response>
    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var getProduct = _prodDAL.GetProductWithInclude(e => e.Id == id);
        if (getProduct is not null)
        {
            return Ok(getProduct);
        }
        return NotFound("Product iD not found.");
    }


    /// <summary>
    /// Create a Product at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="201">If the creation was successful</response>
    [HttpPost]
    public IActionResult PostProduct([FromBody] ProductDto productDto,
        [FromServices] DAL<ProductCategory> categoryDAL)
    {
        var getProduct = _prodDAL.GetBy(e => e.Name.ToLower() == productDto.Name.ToLower());
        if (getProduct is not null)
        {
            return Conflict("Product with this Name already exists.");
        }

        //Check if Category exists in database
        var categoryCheck = categoryDAL.GetBy(c => c.Name.ToLower() == productDto.Category?.Name.ToLower());
        if (categoryCheck is null)
        {
            return NotFound("Category not found in database.");
        }
        //
        var product = _mapper.Map<Product>(productDto);
        product.Category = categoryCheck;
        _prodDAL.Create(product);
        return CreatedAtAction(nameof(GetProductById),
            new { id = product.Id }, product);
    }



    /// <summary>
    /// Update a Product at Database
    /// </summary>
    /// <param name="productDto">Object with the neccessary fields</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    [HttpPut("{id}")]
    public IActionResult UpdateProduct([FromBody] ProductDto productDto,
        [FromServices] DAL<ProductCategory> categoryDAL,
        int id)
    {
        var getProduct = _prodDAL.GetBy(e => e.Id.Equals(id));
        if (getProduct is null)
        {
            return NotFound("Product iD not Found.");
        }

        //Check if Category exists in database
        var categoryCheck = categoryDAL.GetBy(c => c.Name.ToLower() == productDto.Category.Name.ToLower());
        if (categoryCheck is null)
        {
            return NotFound("Category not found in database.");
        }
        //

        _mapper.Map(productDto, getProduct);
        getProduct.ChangeProductCategory(categoryCheck);
        _prodDAL.Update(getProduct);
        return NoContent();
    }

    /// <summary>
    /// Update a Product field at Database
    /// </summary>
    /// <param name="patch">Object with the neccessary fields</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the update was successful</response>
    [HttpPatch]
    public IActionResult PatchProduct(int id,
        JsonPatchDocument<PatchProductDto> patch)
    {
        var getProduct = _prodDAL.GetBy(e => e.Id == id);
        if (getProduct is null)
            return NotFound("Product iD not Found.");

        var productToUpdate = _mapper.Map<PatchProductDto>(getProduct);
        patch.ApplyTo(productToUpdate, ModelState);
        if (!TryValidateModel(productToUpdate)) return ValidationProblem(ModelState);

        _mapper.Map(productToUpdate, getProduct);
        _prodDAL.Update(getProduct);
        return NoContent();
    }


    /// <summary>
    /// Delete a Product at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="204">If the delete was successful</response>
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct([FromServices] DAL<Product> _prodDAL, int id)
    {
        var getProduct = _prodDAL.GetBy(e => e.Id.Equals(id));
        if (getProduct is null)
        {
            return NotFound("Product iD not Found.");
        }
        _prodDAL.Delete(getProduct);
        return NoContent();
    }

}