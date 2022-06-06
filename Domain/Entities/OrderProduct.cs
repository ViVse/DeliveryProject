using Domain.Common;

namespace Domain.Entities
{
    public class OrderProduct: BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
