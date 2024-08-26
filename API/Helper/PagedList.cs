
using Microsoft.EntityFrameworkCore;

namespace API.Helper
{
    public class PagedList<T> : List<T>
    {
        public PagedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Metadata = new Metadata
            {
                Count = count,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }

        public Metadata Metadata { get; set; }

        public static async Task<PagedList<T>> ToPageList(IQueryable<T> query, int pageIndex, int pageSize)
        {
            var count = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}