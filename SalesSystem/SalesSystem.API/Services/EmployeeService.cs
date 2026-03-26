using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SalesSystem.API.Enum;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;
using SalesSystem.Shared.Database.Entities;
using SalesSystem.Shared.Database.Responses;

namespace SalesSystem.API.Services;
/// <summary>
/// Service responsible to manage Employee Controller
/// </summary>
/// <param name="mapper"></param>
/// <param name="empDAL"></param>
/// <param name="userManager"></param>
public class EmployeeService(IMapper mapper, DAL<Employee> empDAL, UserManager<ApplicationUser> userManager)
{
    private readonly IMapper _mapper = mapper;
    private readonly DAL<Employee> _empDAL = empDAL;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    internal async Task<ResultService<PagedResult<GetEmployeeDto>>> GetAllAsync(int skip, int take)
    {
        var getEmployees = await _empDAL.GetAllPagedAsync(skip, take);
        if (getEmployees.Data is null || getEmployees.Data.Count == 0)
            return ResultService<PagedResult<GetEmployeeDto>>.Fail("There's no Employees in database.", ResultStatus.NoContent);

        var dto = _mapper.Map<PagedResult<GetEmployeeDto>>(getEmployees);

        return ResultService<PagedResult<GetEmployeeDto>>.Ok(dto, ResultStatus.Success);
    }
    internal async Task<ResultService<GetEmployeeDto>> GetByIdAsync(int id)
    {
        var getEmployee = await _empDAL.GetByAsync(e => e.Id == id);
        if (getEmployee is null)
            return ResultService<GetEmployeeDto>.Fail("Employee not found.", ResultStatus.NotFound);

        var dto = _mapper.Map<GetEmployeeDto>(getEmployee);
        return ResultService<GetEmployeeDto>.Ok(dto, ResultStatus.Success);
    }
    internal async Task<ResultService<GetEmployeeDto>> CreateAsync(EmployeeDto empDto)
    {
        var getEmployee = await _empDAL.GetByAsync(e => e.Email == empDto.Email);
        if (getEmployee is not null)
            return ResultService<GetEmployeeDto>.Fail("Employee with this email already exists.", ResultStatus.Conflict);

        var user = new ApplicationUser
        {
            UserName = empDto.Email,
            Email = empDto.Email
        };

        var result = await _userManager.CreateAsync(user, empDto.Password);
        if (!result.Succeeded)
            return ResultService<GetEmployeeDto>.Fail(result.Errors?.ToString()!, ResultStatus.Error);

        await _userManager.AddToRoleAsync(user, Roles.Employee);

        var employee = _mapper.Map<Employee>(empDto);

        await _empDAL.CreateAsync(employee);

        var getDto = _mapper.Map<GetEmployeeDto>(employee);
        return ResultService<GetEmployeeDto>.Ok(getDto, ResultStatus.Created);
    }
    internal async Task<ResultService<GetEmployeeDto>> UpdateAsync(int id, EmployeeDto employeeUpdated)
    {
        var getEmployee = await _empDAL.GetByAsync(e => e.Id.Equals(id));
        var user = await _userManager.FindByEmailAsync(getEmployee?.Email!);

        if (getEmployee is null || user is null)
            return ResultService<GetEmployeeDto>.Fail("Employee not Found.", ResultStatus.NotFound);

        //Changes Employee in Database
        var nameValidator = getEmployee.TryChangeEmployeeName(employeeUpdated.Name);
        var emailValidator = getEmployee.TryChangeEmployeeEmail(employeeUpdated.Email);

        if (!nameValidator.Success)
            return ResultService<GetEmployeeDto>.Fail(nameValidator.Error!, ResultStatus.BadRequest);

        if (!emailValidator.Success)
            return ResultService<GetEmployeeDto>.Fail(emailValidator.Error!, ResultStatus.BadRequest);

        //Changes Employee User in IdentityDB
        user.UserName = employeeUpdated.Email;
        user.Email = employeeUpdated.Email;
        await _userManager.UpdateAsync(user);
        //Change Password
        if (!string.IsNullOrEmpty(employeeUpdated.Password))
        {
            //Change Password in IdentityDB
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var tryPasswordChange = await _userManager.ResetPasswordAsync(
                user,
                token,
                employeeUpdated.Password);

            if (!tryPasswordChange.Succeeded)
                return ResultService<GetEmployeeDto>.Fail(tryPasswordChange.Errors.ToString()!, ResultStatus.BadRequest);
        }

        _empDAL.Update(getEmployee);
        return ResultService<GetEmployeeDto>.Ok(null!, ResultStatus.NoContent);
    }
    internal async Task<ResultService<PatchEmployeeDto>> PatchAsync(int id, PatchEmployeeDto empToUpdate)
    {
        var getEmployee = await _empDAL.GetByAsync(e => e.Id.Equals(id));
        var user = await _userManager.FindByEmailAsync(getEmployee?.Email!);
        if (getEmployee is null || user is null)
            return ResultService<PatchEmployeeDto>.Fail("Employee not Found.", ResultStatus.NotFound);

        _mapper.Map(empToUpdate, getEmployee);

        // Updates Identity if Email's changed
        if (user.Email != empToUpdate.Email)
        {
            user.Email = empToUpdate.Email;
            user.UserName = empToUpdate.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return ResultService<PatchEmployeeDto>.Fail(result.Errors.ToString()!, ResultStatus.BadRequest);
        }

        _empDAL.Update(getEmployee);
        return ResultService<PatchEmployeeDto>.Ok(null!, ResultStatus.NoContent);
    }
    internal async Task<ResultService<PatchEmployeeDto>> ChangePassword(int id, ChangePasswordDto empDto)
    {
        var getEmployee = await _empDAL.GetByAsync(e => e.Id == id);
        var user = await _userManager.FindByEmailAsync(getEmployee?.Email!);

        if (getEmployee is null || user is null)
            return ResultService<PatchEmployeeDto>.Fail("Employee not found.", ResultStatus.NotFound);

        if (String.IsNullOrEmpty(empDto.currentPassword) || String.IsNullOrEmpty(empDto.newPassword))
            return ResultService<PatchEmployeeDto>.Fail("Password can't be null or empty.", ResultStatus.BadRequest);

            var result = await _userManager.ChangePasswordAsync(
                user,
                empDto.currentPassword,
                empDto.newPassword
                );

        if (!result.Succeeded)
            return ResultService<PatchEmployeeDto>.Fail(result.Errors.ToString()!, ResultStatus.Error);

        return ResultService<PatchEmployeeDto>.Ok(null!, ResultStatus.NoContent);
    }
    internal async Task<ResultService<EmployeeDto>> Delete(int id)
    {
        var getEmployee = await _empDAL.GetByAsync(e => e.Id.Equals(id));
        var user = await _userManager.FindByEmailAsync(getEmployee?.Email!);
        if (getEmployee is null || user is null)
        {
            return ResultService<EmployeeDto>.Fail("Employee not Found.", ResultStatus.NotFound);
        }
        _empDAL.Delete(getEmployee);
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return ResultService<EmployeeDto>.Fail("Failure trying to delete Employee", ResultStatus.BadRequest);

        return ResultService<EmployeeDto>.Ok(null!, ResultStatus.NoContent);
    }
}
