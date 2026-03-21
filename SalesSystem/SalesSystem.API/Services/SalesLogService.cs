using SalesSystem.API.Enum;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos;

namespace SalesSystem.API.Services;

/// <summary>
/// Service responsible to manage SalesLog Controller
/// </summary>
/// <param name="empDal"></param>
/// <param name="prodDal"></param>
/// <param name="logDal"></param>
public class SalesLogService(DAL<Employee> empDal, DAL<Product> prodDal, DAL<SalesLog> logDal)
{
    private readonly DAL<Employee> _empDal = empDal;
    private readonly DAL<Product> _prodDal = prodDal;
    private readonly DAL<SalesLog> _slogDal = logDal;

    /// <summary>
    /// Tries to create a new SalesLog
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public Result<SalesLog> Create(SalesLogDto dto)
    {
        var employee = _empDal.GetBy(e => e.Id == dto.employeeId);
        if (employee is null)
            return Result<SalesLog>.Fail("Employee not found in Database", ResultStatus.NotFound);

        var product = _prodDal.GetBy(p => p.Id == dto.productId);
        if (product is null)
            return Result<SalesLog>.Fail("Product not found in Database", ResultStatus.NotFound);

        if (dto.quantity <= 0)
            return Result<SalesLog>.Fail("Quantity not acceptable", ResultStatus.BadRequest);

        if (dto.quantity > product.Quantity)
            return Result<SalesLog>.Fail($"There's no specified quantity of {product.Name}", ResultStatus.BadRequest);

        var totalPrice = dto.quantity * product.Price;

        var log = new SalesLog(employee, product, dto.quantity, totalPrice);
        if (log is null)
            return Result<SalesLog>.Fail("Something went wrong trying to create SalesLog", ResultStatus.BadRequest);

        var result = product.TryRemoveStock(dto.quantity);
        if (!result.Success)
            return Result<SalesLog>.Fail("Was not possible to update this Product quantity", ResultStatus.BadRequest);

        _prodDal.Update(product);
        _slogDal.Create(log);
        return Result<SalesLog>.Ok(log, ResultStatus.Success);
    }
}
