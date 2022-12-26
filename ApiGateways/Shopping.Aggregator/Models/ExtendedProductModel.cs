namespace Shopping.Aggregator.Models
{
    public class ExtendedProductModel
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public ShopModel Shop { get; set; }
    }
}
