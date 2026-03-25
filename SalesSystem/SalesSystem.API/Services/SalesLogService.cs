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

    internal ResultService<ICollection<SalesLog>> GetAll(int skip, int take)
    {
        var getLog = _slogDal.GetAllPaged(skip, take);
        if (getLog is null || getLog.Count == 0)
        {
            return ResultService<ICollection<SalesLog>>.Fail("SalesLog not found!", ResultStatus.NotFound);
        }
        return ResultService<ICollection<SalesLog>>.Ok(getLog, ResultStatus.Success);
    }
    internal ResultService<SalesLog> GetById(Guid id)
    {
        var getLog = _slogDal.GetBy(s => s.SaleId == id);
        if (getLog is null)
        {
            return ResultService<SalesLog>.Fail("Log iD not found!", ResultStatus.NotFound);
        }

        return ResultService<SalesLog>.Ok(getLog, ResultStatus.Success);
    }
    internal ResultService<ICollection<SalesLog>> GetByEmployeeId(int employeeId)
    {
        var getEmployee = _empDal.GetBy(e => e.Id == employeeId);
        if (getEmployee is null)
            return ResultService<ICollection<SalesLog>>.Fail("Employee not found.", ResultStatus.NotFound);

        var getLog = _slogDal.GetAllBy(l => l.EmployeeId == employeeId);
        if (getLog is null || getLog.Count == 0)
            return ResultService<ICollection<SalesLog>>.Fail("Employee hasn't sales.", ResultStatus.NotFound);

        return ResultService<ICollection<SalesLog>>.Ok(getLog, ResultStatus.Success);
    }
    internal ResultService<ICollection<SalesLog>> GetByDate(int startYear, int finalYear)
    {
        var getLog = _slogDal.GetAllBy(s => s.Time.Year >= startYear && s.Time.Year <= finalYear);
        if (getLog is null || getLog.Count == 0)
        {
            return ResultService<ICollection<SalesLog>>.Fail("Total of 0 Logs found!", ResultStatus.NotFound);
        }
        return ResultService<ICollection<SalesLog>>.Ok(getLog, ResultStatus.Success);
    }
    internal ResultService<SalesLog> Create(SalesLogDto dto)
    {
        var employee = _empDal.GetBy(e => e.Id == dto.employeeId);
        if (employee is null)
            return ResultService<SalesLog>.Fail("Employee not found in Database", ResultStatus.NotFound);

        var product = _prodDal.GetBy(p => p.Id == dto.productId);
        if (product is null)
            return ResultService<SalesLog>.Fail("Product not found in Database", ResultStatus.NotFound);

        if (dto.quantity <= 0)
            return ResultService<SalesLog>.Fail("Quantity not acceptable", ResultStatus.BadRequest);

        if (dto.quantity > product.Quantity)
            return ResultService<SalesLog>.Fail($"There's no specified quantity of {product.Name}", ResultStatus.BadRequest);

        var totalPrice = dto.quantity * product.Price;

        var log = new SalesLog(employee, product, dto.quantity, totalPrice);
        if (log is null)
            return ResultService<SalesLog>.Fail("Something went wrong trying to create SalesLog", ResultStatus.BadRequest);

        var result = product.TryRemoveStock(dto.quantity);
        if (!result.Success)
            return ResultService<SalesLog>.Fail("Was not possible to update this Product quantity", ResultStatus.BadRequest);

        _prodDal.Update(product);
        _slogDal.Create(log);
        return ResultService<SalesLog>.Ok(log, ResultStatus.Success);
    }

}
