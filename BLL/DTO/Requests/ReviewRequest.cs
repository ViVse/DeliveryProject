namespace BLL.DTO.Requests
{
    public class ReviewRequest
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Stars { get; set; }
        public DateTime Date { get; set; }
        public int ShopId { get; set; }
        public string CustomerId { get; set; }
    }
}
