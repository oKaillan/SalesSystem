using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SalesSystem.API.Enum;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos;
using SalesSystem.Shared.Database.Database.Dtos.FilterDto;
using SalesSystem.Shared.Database.Database.Dtos.ProductCategoryDto;
using SalesSystem.Shared.Database.Database.Dtos.ProductDto;
using SalesSystem.Shared.Database.Responses;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace SalesSystem.API.Services;

/// <summary>
/// Class responsible to manage Product Controller
/// </summary>
/// <param name="mapper"></param>
/// <param name="prodDAL"></param>
/// <param name="pCategoryDAL"></param>
public class ProductService(IMapper mapper, DAL<Product> prodDAL, DAL<ProductCategory> pCategoryDAL)
{
    private readonly IMapper _mapper = mapper;
    private readonly DAL<Product> _prodDAL = prodDAL;
    private readonly DAL<ProductCategory> _pCategoryDAL = pCategoryDAL;
    //This is the Product GET Model, used in Get all Products and Get by iD
    private readonly Expression<Func<Product, GetProductDto>> getProductDto = p => new GetProductDto(
        p.Id,
        p.Name,
        p.Quantity,
        p.Price,
        p.Categories.Select(c => new GetCategoryDto(c.Id, c.Name))
        );

    internal async Task<ResultService<PagedResult<GetProductDto>>> GetAllAsync(int skip, int take, ProductFilterDto? search)
    {
        Expression<Func<Product, bool>>? filter = null;

        if (search is not null)
        {
            filter = p =>
            (string.IsNullOrEmpty(search.Name) || p.Name.ToLower().Contains(search.Name.ToLower())) &&
            (!search.CategoryId.HasValue || p.Categories.Any(c => c.Id == search.CategoryId));
        }

        Console.WriteLine($"Name: {search.Name}");
        Console.WriteLine($"Category: {search.CategoryId}");

        var getProducts = await _prodDAL.GetAllPagedWithSelectorAsync(skip, take, filter, getProductDto, p => p.Categories);
        if (getProducts is null)
        {
            return ResultService<PagedResult<GetProductDto>>.Fail("There's no Products in database.", ResultStatus.NoContent);
        }
        return ResultService<PagedResult<GetProductDto>>.Ok(getProducts, ResultStatus.Success);
    }
    internal async Task<ResultService<GetProductDto>> GetByIdAsync(int id)
    {
        var getProduct = await _prodDAL.GetByAsync(e => e.Id == id);
        if (getProduct is null)
        {
            return ResultService<GetProductDto>.Fail("Product iD not found.", ResultStatus.NotFound);
        }

        var dto = _mapper.Map<GetProductDto>(getProduct);
        return ResultService<GetProductDto>.Ok(dto, ResultStatus.Success);
    }
    internal async Task<ResultService<GetProductDto>> CreateAsync(ProductDto productDto)
    {
        var getProduct = await _prodDAL.GetByAsync(e => e.Name.ToLower() == productDto.Name.ToLower());
        if (getProduct is not null)
        {
            return ResultService<GetProductDto>.Fail("Product with this Name already exists.", ResultStatus.Conflict);
        }

        //Checks if atleast one category was informed
        if (!productDto.CategoryIds.Any() || !productDto.CategoryIds.Any())
            return ResultService<GetProductDto>.Fail("At least one category must be provided.", ResultStatus.BadRequest);

        //Checks if category exists in Database
        var existingCategories = await _pCategoryDAL.GetAllByAsync(c => productDto.CategoryIds.Contains(c.Id));

        if (existingCategories.Count != productDto.CategoryIds.Count)
        {
            var missingIds = productDto.CategoryIds.Except(existingCategories.Select(c => c.Id));
            return ResultService<GetProductDto>.Fail(
                $"Categories not found in database: {string.Join(", ", missingIds)}",
                ResultStatus.NotFound
                );
        }

        //Creates Product
        var product = _mapper.Map<Product>(productDto);
        product.Categories = existingCategories;


        await _prodDAL.CreateAsync(product);

        var getDto = _mapper.Map<GetProductDto>(product);
        return ResultService<GetProductDto>.Ok(getDto, ResultStatus.Created);
    }
    internal async Task<ResultService<GetProductDto>> UpdateAsync(ProductDto productDto, int id)
    {
        var getProduct = await _prodDAL.GetByAsync(e => e.Id.Equals(id));
        if (getProduct is null)
        {
            return ResultService<GetProductDto>.Fail("Product iD not Found.", ResultStatus.NotFound);
        }

        //Check if Category exists in database
        var categoryCheck = await _pCategoryDAL.GetAllByAsync(c => productDto.CategoryIds.Contains(c.Id));
        if (!categoryCheck.Any() || categoryCheck.Count == 0)
        {
            return ResultService<GetProductDto>.Fail("One or more Categories not found in database.", ResultStatus.NotFound);
        }
        //

        _mapper.Map(productDto, getProduct);
        getProduct.TryChangeProductCategories(categoryCheck);
        _prodDAL.Update(getProduct);
        return ResultService<GetProductDto>.Ok(null!, ResultStatus.NoContent);
    }
    internal async Task<ResultService<GetProductDto>> PatchAsync(int id,
        JsonPatchDocument<PatchProductDto> patch)
    {
        var getProduct = await _prodDAL.GetByAsync(e => e.Id == id);
        if (getProduct is null)
            return ResultService<GetProductDto>.Fail("Product iD not Found.", ResultStatus.NotFound);

        var productToUpdate = _mapper.Map<PatchProductDto>(getProduct);
        patch.ApplyTo(productToUpdate);
        var context = new ValidationContext(productToUpdate);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(productToUpdate, context, results, true))
            return ResultService<GetProductDto>.Fail(results.ToString()!, ResultStatus.Error);

        var checkCategory = await _pCategoryDAL.GetAllByAsync(c => productToUpdate.Categories!.Contains(c.Id));

        if (!checkCategory.Any()) // If categories was not found, it backs to previous one
            checkCategory = (List<ProductCategory>?)getProduct.Categories;

        _mapper.Map(productToUpdate, getProduct);

        getProduct.TryChangeProductCategories(checkCategory!); // Always changing, to not create a new one

        _prodDAL.Update(getProduct);
        return ResultService<GetProductDto>.Ok(null!, ResultStatus.NoContent);
    }
    internal async Task<ResultService<GetProductDto>> DeleteAsync(int id)
    {
        var getProduct = await _prodDAL.GetByAsync(e => e.Id.Equals(id));
        if (getProduct is null)
        {
            return ResultService<GetProductDto>.Fail("Product iD not Found.", ResultStatus.NotFound);
        }
        _prodDAL.Delete(getProduct);
        return ResultService<GetProductDto>.Ok(null!, ResultStatus.NoContent);
    }
}
