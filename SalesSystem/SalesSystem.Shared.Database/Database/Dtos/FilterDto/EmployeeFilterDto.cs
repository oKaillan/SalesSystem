using SalesSystem.Entities;
using SalesSystem.Shared.Database.Enum;
using SalesSystem.Shared.Database.Interfaces;

namespace SalesSystem.Shared.Database.Database.Dtos.FilterDto;

public class EmployeeFilterDto : IFilterDto<EmployeeOrderByFilter>
{

    public string? NameOrEmail { get; set; }
    public EmployeeOrderByFilter? OrderBy { get; set; }
    public bool Descending { get; set; } = false;

    public EmployeeFilterDto() { }
    public EmployeeFilterDto(string? nameOrEmail, EmployeeOrderByFilter? orderBy, bool desc = false)
    {
        NameOrEmail = nameOrEmail;
        Descending = desc;
        OrderBy = orderBy;
    }
}
