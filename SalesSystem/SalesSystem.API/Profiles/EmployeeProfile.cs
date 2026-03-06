using AutoMapper;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Database.Dtos.EmployeeDto;

namespace SalesSystem.API.Profiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeDto, Employee>();
        CreateMap<Employee, EmployeeDto>();
        CreateMap<PatchEmployeeDto, Employee>();
        CreateMap<Employee, PatchEmployeeDto>();
        CreateMap<PostEmployeeDto,  Employee>();
    }
}
