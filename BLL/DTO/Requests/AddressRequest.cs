namespace BLL.DTO.Requests
{
    public class AddressRequest
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string AddressLine { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
