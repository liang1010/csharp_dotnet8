namespace ht_csharp_dotnet8.Models
{
    public class PageListingRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public PageListingRequest()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public PageListingRequest(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 1 ? 1 : pageSize;
        }
    }
}
