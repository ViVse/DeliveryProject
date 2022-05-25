using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class User: IdentityUser 
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Avatar { get; set; }

        public int DefaultAddressId { get; set; }

        public Address DefaultAddress { get; set; }
    }
}
