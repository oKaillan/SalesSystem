using Microsoft.AspNetCore.Components;

namespace SalesSystem.Web.Services;

public class PaginationService<T> where T : class
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

    public async Task Paginate()
    {
        var result = await _api.GetObjectPagedListAsync(
            _path,
            Skip,
            ObjectsPerPage
            );

        Data = result.Data;
        TotalCount = result.TotalCount;
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
