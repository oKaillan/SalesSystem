using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.Shared.Database.Responses
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = null!;
        public int TotalCount { get; set; }
    }
}
