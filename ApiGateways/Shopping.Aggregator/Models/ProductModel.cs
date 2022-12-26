namespace Shopping.Aggregator.Models
{
    public class ProductModel
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
    }
}
