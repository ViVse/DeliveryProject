namespace BLL.DTO.Responses
{
    public class ShopResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int AddressId { get; set; }
        public string City { get; set; }
        public string AddressLine { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
