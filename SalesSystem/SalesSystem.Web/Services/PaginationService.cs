using Microsoft.AspNetCore.Components;
using SalesSystem.Shared.Database.Interfaces;

namespace SalesSystem.Web.Services;

public class PaginationService<T, TOrderBy> 
    where T : class
    where TOrderBy : struct
{
    private readonly BaseApiService<T> _api;
    private readonly string _path;

    public PaginationService(BaseApiService<T> api, string path, int pageSize)
    {
        _api = api;
        _path = path;
        ObjectsPerPage = pageSize;
    }

    public int PageNumber { get; private set; } = 1;
    public int ObjectsPerPage { get; }
    public int TotalCount { get; private set; }

    public List<T> Data { get; private set; } = null!;

    public int Skip => (PageNumber - 1) * ObjectsPerPage;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / ObjectsPerPage);
    private IFilterDto<TOrderBy>? _currentFilter;

    public async Task Paginate(IFilterDto<TOrderBy>? filter = null)
    {
        _currentFilter = filter ?? _currentFilter;

        if (filter != null)
            PageNumber = 1;

        var result = await _api.GetObjectPagedListAsync(
            _path,
            Skip,
            ObjectsPerPage,
            _currentFilter
            );

        Data = result.Data;
        TotalCount = result.TotalCount;
    }

    public void SetSingleItem(T item)
    {
        if (item is null)
        {
            Data = new List<T>();
            TotalCount = 0;
            PageNumber = 1;
            return;
        }
        Data = new List<T> { item };
        TotalCount = 1;
        PageNumber = 1;
    }

    public async Task ResetAsync()
    {
        PageNumber = 1;
        await Paginate();
    }

    public async Task GoToPageNumberAsync(ChangeEventArgs e)
    {
        //Convert input value to int and saves in number var
        if (int.TryParse(e.Value?.ToString(), out int number))
        {
            if (number <= TotalPages || number != PageNumber || number >= 1)
            {
                PageNumber = number;
            }
            if (number < 1)
            {
                PageNumber = 1;
            }
            if (number > TotalPages)
            {
                PageNumber = TotalPages;
            }
        }

        await Paginate();
    }

    public async Task NextPageAsync()
    {
        if (PageNumber < TotalPages)
        {
            PageNumber++;
            await Paginate();
        }
    }

    public async Task PreviousPageAsync()
    {
        if (PageNumber > 1)
        {
            PageNumber--;
            await Paginate();
        }
    }
}
