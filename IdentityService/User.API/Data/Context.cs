using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace User.API.Data
{
    public class Context : IdentityDbContext<UserModel>
    {
        public Context(DbContextOptions options) : base(options)
        {
        }
    }
}
