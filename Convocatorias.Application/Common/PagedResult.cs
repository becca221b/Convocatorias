using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.Common
{
    public sealed class PagedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public PagedResult(int page, int pageSize, int totalItems, IReadOnlyCollection<T> items)
        {
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = items;
            
        }
    }
}
