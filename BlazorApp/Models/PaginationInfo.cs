namespace BlazorApp.Models
{
    public class PaginationInfo
    {
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public bool hasNext { get; set; }
        public bool hasPrevious { get; set; }
    }
}
