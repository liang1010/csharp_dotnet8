namespace ht_csharp_dotnet8.Models
{
    public class PagedListingResponse<T>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public List<T>? Data { get; set; }
        public PagedListingResponse()
        {
            Data = new List<T>();
        }
    }
}
