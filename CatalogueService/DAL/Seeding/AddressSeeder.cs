using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Seeding
{
    public class AddressSeeder: ISeeder<Address>
    {
        private static readonly List<Address> _addresses = new ()
        {
            new Address
            {
                Id = 1,
                City = "Kyiv",
                AddressLine = "street #23",
                Latitude = 25,
                Longitude = 23
            },
            new Address
            {
                Id = 2,
                City = "Chernivtsi",
                AddressLine = "street #2309",
                Latitude = 10,
                Longitude = 5
            },
            new Address
            {
                Id = 3,
                City = "Lviv",
                AddressLine = "street #3",
                Latitude = 15,
                Longitude = 23
            },
            new Address
            {
                Id = 4,
                City = "Dnipro",
                AddressLine = "street #23",
                Latitude = 25,
                Longitude = 23
            }
        };

        public void Seed(EntityTypeBuilder<Address> builder) => builder.HasData(_addresses);
    }
}
