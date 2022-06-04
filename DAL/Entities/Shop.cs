namespace DAL.Entities
{
    public class Shop: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public List<Product> Products { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
