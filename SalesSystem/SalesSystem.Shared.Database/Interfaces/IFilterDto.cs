namespace SalesSystem.Shared.Database.Interfaces;

public interface IFilterDto<TOrderBy> where TOrderBy : struct
{
    public TOrderBy? OrderBy { get; set; }
    public bool Descending { get; set; }
}
