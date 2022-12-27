using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class AspNetIdentityDbContext: IdentityDbContext<UserModel>
    {
        public AspNetIdentityDbContext(DbContextOptions<AspNetIdentityDbContext> options): base(options)
        {

        }
    }
}
