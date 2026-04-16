using AutoMapper;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;
using SalesSystem.Shared.Database.Responses;

namespace SalesSystem.API.Profiles;
/// <summary>
/// Class responsible to map Employees using AutoMapper
/// </summary>
public class EmployeeProfile : Profile
{
    /// <summary>
    /// Maps Employees
    /// </summary>
    public EmployeeProfile()
    {
        CreateMap<GetEmployeeDto, Employee>();
        CreateMap<Employee, GetEmployeeDto>();
        CreateMap<PagedResult<Employee>, PagedResult<GetEmployeeDto>>();
        CreateMap<PatchEmployeeDto, Employee>();
        CreateMap<PatchEmployeeDto, GetEmployeeDto>();
        CreateMap<GetEmployeeDto, PatchEmployeeDto>();
        CreateMap<Employee, PatchEmployeeDto>();
        CreateMap<EmployeeDto,  Employee>();
    }
}
