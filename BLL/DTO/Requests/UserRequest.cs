namespace BLL.DTO.Requests
{
    public class UserRequest
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int DefaultAddressId { get; set; }
    }
}
