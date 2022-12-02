namespace Discount.API.Entities
{
    public class ProductDiscount
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public bool IsPercent { get; set; }
        public int Amount { get; set; }
    }
}
