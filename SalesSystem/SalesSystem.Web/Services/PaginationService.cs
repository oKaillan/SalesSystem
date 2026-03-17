using Microsoft.AspNetCore.Components;

namespace SalesSystem.Web.Services;

public class PaginationService<T> where T : class
{
    public PaginationService(int objectsPerPage, ICollection<T>? objectList)
    {
        PageNumber = 1;
        ObjectsPerPage = objectsPerPage;
        ObjectList = objectList;
        TotalPages = (int)Math.Ceiling((double)objectList!.Count / ObjectsPerPage);

        Paginate();
    }

    public int PageNumber { get; private set; }
    public int ObjectsPerPage { get; private set; }
    public int TotalPages { get; private set; }
    public ICollection<T>? ObjectList { get; private set; }
    public ICollection<T>? ObjectPaginatedList { get; private set; }


    public void Paginate()
    {
        ObjectPaginatedList =
         ObjectList?
        .Skip((PageNumber - 1) * ObjectsPerPage)
        .Take(ObjectsPerPage)
        .ToList();
    }

    public void GoToPageNumber(ChangeEventArgs e)
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

        Paginate();
    }

    public void NextPage()
    {
        if (PageNumber < TotalPages)
        {
            PageNumber++;
            Paginate();
        }
    }

    public void PreviousPage()
    {
        if (PageNumber > 1)
        {
            PageNumber--;
            Paginate();
        }
    }
}
