using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class BasketCheckoutEvent: IntegrationBaseEvent
    {
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }

        public string AddressLine { get; set; }
        public List<Product> Products { get; set; }

        //Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
    }

    public class Product
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
    }
}
