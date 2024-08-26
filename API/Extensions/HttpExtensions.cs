
using API.Helper;
using Newtonsoft.Json;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, Metadata metaData)
        {
            var paginationHeader = new
            {
                metaData.CurrentPage,
                metaData.PageSize,
                metaData.Count,
                metaData.TotalPages
            };
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}