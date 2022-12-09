﻿namespace Cart.API.Entities
{
    public class Checkout
    {
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }

        public string AddressLine { get; set; }
        public List<ShoppingCartItem> Products { get; set; }

        //Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
    }
}
