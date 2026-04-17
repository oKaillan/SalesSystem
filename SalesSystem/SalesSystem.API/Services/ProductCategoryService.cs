using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Responses;
using SalesSystem.API.Enum;
using AutoMapper;
using SalesSystem.Shared.Database.Database.Dtos.ProductCategoryDto;

namespace SalesSystem.API.Services;
/// <summary>
/// Service responsible to manage ProductCategory Controller
/// </summary>
/// <param name="pCategoryDal"></param>
/// <param name="mapper"></param>
public class ProductCategoryService(DAL<ProductCategory> pCategoryDal, IMapper mapper)
{
    private readonly DAL<ProductCategory> _pCategoryDal = pCategoryDal;
    private readonly IMapper _mapper = mapper;

    internal async Task<ResultService<PagedResult<GetCategoryDto>>> GetAllAsync(int skip, int take)
    {
        var categories = await _pCategoryDal.GetAllPagedAsync(skip, take);
        if (categories.TotalCount == 0 || categories.Data.Count == 0)
            return ResultService<PagedResult<GetCategoryDto>>.Fail("There's no Categories in database!", ResultStatus.NoContent);

        var dto = _mapper.Map<PagedResult<GetCategoryDto>>(categories);

        return ResultService<PagedResult<GetCategoryDto>>.Ok(dto, ResultStatus.Success);
    }
    internal async Task<ResultService<GetCategoryDto>> GetByAsync(int id)
    {
        var getCategory = await _pCategoryDal.GetByAsync(c => c.Id == id);
        if (getCategory is null)
            return ResultService<GetCategoryDto>.Fail("There's no category with this id.", ResultStatus.NotFound);

        var dto = _mapper.Map<GetCategoryDto>(getCategory);

        return ResultService<GetCategoryDto>.Ok(dto, ResultStatus.Success);
    }
    internal async Task<ResultService<GetCategoryDto>> CreateAsync(CreateCategoryDto pCategory)
    {
        var categoryCheck = await _pCategoryDal.GetByAsync(c => c.Name.ToLower() == pCategory.name.ToLower()); // Checks if Category already exists
        if (categoryCheck is not null)
        {
            return ResultService<GetCategoryDto>.Fail("Category already exist.", ResultStatus.Conflict);
        }

        var categoryConverted = _mapper.Map<ProductCategory>(pCategory);

        await _pCategoryDal.CreateAsync(categoryConverted);

        var dto = _mapper.Map<GetCategoryDto>(categoryConverted);

        return ResultService<GetCategoryDto>.Ok(dto, ResultStatus.Created);
    }
    internal async Task<ResultService<GetCategoryDto>> UpdateAsync(CreateCategoryDto pCategory, int id)
    {
        var getCategory = await _pCategoryDal.GetByAsync(c => c.Id == id);
        if (getCategory is null)
        {
            return ResultService<GetCategoryDto>.Fail("Category does not exist.", ResultStatus.NotFound);
        }

        getCategory.ChangeCategoryName(pCategory.name);
        _pCategoryDal.Update(getCategory);
        return ResultService<GetCategoryDto>.Ok(null!, ResultStatus.NoContent);
    }
    internal async Task<ResultService<GetCategoryDto>> DeleteAsync(int id)
    {
        var categoryCheck = await _pCategoryDal.GetByAsync(p => p.Id == id);
        if (categoryCheck is null)
        {
            return ResultService<GetCategoryDto>.Fail("Category does not exist.", ResultStatus.NotFound);
        }

        _pCategoryDal.Delete(categoryCheck);
        return ResultService<GetCategoryDto>.Ok(null!, ResultStatus.NoContent);
    }
}
