namespace SalesSystem.Shared.Database.Database.Dtos.FilterDto;

public class EmployeeFilterDto
{

    public string? Name { get; set; }
    public string? Email { get; set; }
    public bool? Desc { get; set; } = false;

    public EmployeeFilterDto(string? name, string? email, bool? desc)
    {
        Name = name;
        Email = email;
        Desc = desc;
    }
}
