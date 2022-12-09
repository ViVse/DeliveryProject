namespace Cart.API.Requests
{
    public class CheckoutRequest
    {
        public string UserId { get; set; }
        public string AddressLine { get; set; }

        //Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
    }
}
