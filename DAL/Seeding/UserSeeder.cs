using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Seeding
{
    public class UserSeeder: ISeeder<User>
    {
        private static readonly List<(User, string)> users = new()
        {
            (
            new User()
            {
                Id = "3b333929-f974-444e-a8d3-68f50a0459c0",
                FirstName = "Ivan",
                LastName = "Ishchenko",
                Email = "Test@gmail.com",
                PhoneNumber = "0950907774",
                Avatar = "",
                DefaultAddressId = 1

            },
            "User%password1"
            ),
            (
            new User()
            {
                Id = "3b333929-f974-444e-a8d3-68f50a0456g1",
                FirstName = "Vova",
                LastName = "Vasenko",
                Email = "new@gmail.com",
                PhoneNumber = "0950907894",
                Avatar = "",
                DefaultAddressId = 2

            },
            "User%password2"
            )
        };

        public void Seed(EntityTypeBuilder<User> builder)
        {
            foreach (var (user, password) in users)
            {
                user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);
                builder.HasData(user);
            }
        }
    }
}
