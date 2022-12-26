namespace Shopping.Aggregator.Models
{
    public class ExtendedOrderModel
    {
        public string Id { get; set; }
        public UserModel User { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public int DeliveryManId { get; set; }
        public string AddressLine { get; set; }
        public List<ExtendedProductModel> Products { get; set; }
    }
}
