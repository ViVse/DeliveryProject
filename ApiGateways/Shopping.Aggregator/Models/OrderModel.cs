namespace Shopping.Aggregator.Models
{
    public class OrderModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public int DeliveryManId { get; set; }
        public string AddressLine { get; set; }
        public List<ProductModel> Products { get; set; }
    }
}
