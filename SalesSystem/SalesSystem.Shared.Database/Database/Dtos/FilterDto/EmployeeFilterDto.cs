using SalesSystem.Shared.Database.Enum;

namespace SalesSystem.Shared.Database.Database.Dtos.FilterDto;

public class EmployeeFilterDto
{

    public string? NameOrEmail { get; set; }
    public EmployeeOrderByFilter? OrderBy {get; set;}
    public bool Desc { get; set; } = false;

    public EmployeeFilterDto(string? nameOrEmail, EmployeeOrderByFilter? orderBy, bool desc = false)
    {
        NameOrEmail = nameOrEmail;
        Desc = desc;
        OrderBy = orderBy;
    }
}
