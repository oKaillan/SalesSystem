using AutoMapper;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;

namespace SalesSystem.API.Profiles;
/// <summary>
/// Controller Responsibly to map Employees using AutoMapper
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
        CreateMap<PatchEmployeeDto, Employee>();
        CreateMap<PatchEmployeeDto, GetEmployeeDto>();
        CreateMap<GetEmployeeDto, PatchEmployeeDto>();
        CreateMap<Employee, PatchEmployeeDto>();
        CreateMap<EmployeeDto,  Employee>();
    }
}
