using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.ProductDto;
using System.Linq.Expressions;

namespace SalesSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly DAL<Product> _prodDAL;
    private readonly DAL<ProductCategory> _pCategoryDAL;
    //This is the Product GET Model, used in Get all Products and Get by iD
    private readonly Func<Product, GetProductDto> getProductDto = p => new GetProductDto
    {
        Id = p.Id,
        Name = p.Name,
        Quantity = p.Quantity,
        Price = p.Price,
        Categories = p.Categories.Select(p => p.Name).ToList()
    };

    public ProductController(IMapper mapper, DAL<Product> prodDAL, DAL<ProductCategory> pCategoryDAL)
    {
        _mapper = mapper;
        _prodDAL = prodDAL;
        _pCategoryDAL = pCategoryDAL;
    }

    /// <summary>
    /// Returns all Products in Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Search was successful</response>
    [HttpGet]
    public IActionResult GetProducts(int skip = 0, int take = 50)
    {
        var getProducts = _prodDAL.GetAllInRange(skip, take)
            .Select(getProductDto);
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
        var getProduct = _prodDAL.GetBy(e => e.Id == id);
        if (getProduct is null)
        {
            return NotFound("Product iD not found.");
        }

        var product = getProductDto(getProduct);
        return Ok(product);
    }

    /// <summary>
    /// Create a Product at Database
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="201">If the creation was successful</response>
    [HttpPost]
    public IActionResult PostProduct([FromBody] ProductDto productDto)
    {
        var getProduct = _prodDAL.GetBy(e => e.Name.ToLower() == productDto.Name.ToLower());
        if (getProduct is not null)
        {
            return Conflict("Product with this Name already exists.");
        }

        //Checks if atleast one category was informed
        if (productDto.CategoryIds is null || !productDto.CategoryIds.Any())
            return BadRequest("At least one category must be provided.");

        //Checks if category exists in Database
        var existingCategories = _pCategoryDAL.GetAllBy(c => productDto.CategoryIds.Contains(c.Id));

        if (existingCategories.Count != productDto.CategoryIds.Count)
        {
            var missingIds = productDto.CategoryIds.Except(existingCategories.Select(c => c.Id));
            return NotFound($"Categories not found in database: {string.Join(", ", missingIds)}");
        }

        //Creates Product
        var product = _mapper.Map<Product>(productDto);
        product.Categories = existingCategories;

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
        int id)
    {
        var getProduct = _prodDAL.GetBy(e => e.Id.Equals(id));
        if (getProduct is null)
        {
            return NotFound("Product iD not Found.");
        }

        //Check if Category exists in database
        var categoryCheck = _pCategoryDAL.GetAllBy(c => productDto.CategoryIds.Contains(c.Id));
        if (categoryCheck is null)
        {
            return NotFound("Category not found in database.");
        }
        //

        _mapper.Map(productDto, getProduct);
        getProduct.ChangeProductCategories(categoryCheck);
        _prodDAL.Update(getProduct);
        return NoContent();
    }

    /// <summary>
    /// Update a Product field at Database
    /// </summary>
    /// <remarks>If Category was not selected or was invalid, it's back to what it was.</remarks>
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

        var checkCategory = _pCategoryDAL.GetAllBy(c => productToUpdate.CategoriesIds.Contains(c.Id));

        if (checkCategory is null) // If categories was not found, it backs to what it was
            checkCategory = (List<ProductCategory>?)getProduct.Categories;
        
        _mapper.Map(productToUpdate, getProduct);

        getProduct.ChangeProductCategories(checkCategory); // Always changing, to not create a new one

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