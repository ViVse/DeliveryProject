namespace DAL.Entities
{
    public class Address: BaseEntity
    {
        public string City { get; set; }
        public string AddressLine { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public List<Shop> Shops { get; set; }
        public List<User> Users { get; set; }
    }
}
