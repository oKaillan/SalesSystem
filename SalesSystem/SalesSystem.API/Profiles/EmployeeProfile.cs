using AutoMapper;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;

namespace SalesSystem.API.Profiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<GetEmployeeDto, Employee>();
        CreateMap<Employee, GetEmployeeDto>();
        CreateMap<PatchEmployeeDto, Employee>();
        CreateMap<Employee, PatchEmployeeDto>();
        CreateMap<EmployeeDto,  Employee>();
    }
}
