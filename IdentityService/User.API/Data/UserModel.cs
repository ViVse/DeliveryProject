using Microsoft.AspNetCore.Identity;

namespace User.API.Data
{
    public class UserModel: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
