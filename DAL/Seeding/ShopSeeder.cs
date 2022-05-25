using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Seeding
{
    public class ShopSeeder: ISeeder<Shop>
    {
        private static readonly List<Shop> shops = new()
        {
            new Shop
            {
                Id = 1,
                Name = "Goods",
                Description = "Nice shop",
                Image = "no image",
                AddressId = 1
            },
            new Shop
            {
                Id = 2,
                Name = "Clothes",
                Description = "Great shop",
                Image = "no image",
                AddressId = 2
            },
            new Shop
            {
                Id = 3,
                Name = "Shoes",
                Description = "Awesome shop",
                Image = "no image",
                AddressId = 3
            },
            new Shop
            {
                Id = 4,
                Name = "Fruit",
                Description = "Bad shop",
                Image = "no image",
                AddressId = 4
            },
        };

        public void Seed(EntityTypeBuilder<Shop> builder) => builder.HasData(shops);
    }
}
