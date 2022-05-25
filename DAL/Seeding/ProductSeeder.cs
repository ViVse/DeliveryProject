using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Seeding
{
    public class ProductSeeder: ISeeder<Product>
    {
        private static readonly List<Product> _products = new ()
        {
            new Product { 
                Id = 1,
                Name = "Banana",
                Description = "Fruit",
                Image = "no image",
                Price = 55,
                ProductionTime = 10,
                ShopId = 3,
            },
            new Product
            {
                Id = 2,
                Name = "Pants",
                Description = "Levis",
                Image = "no image",
                Price = 155,
                ProductionTime = 0,
                ShopId = 2,
            },
            new Product
            {
                Id = 3,
                Name = "Ledder",
                Description = "Useful",
                Image = "no image",
                Price = 35,
                ProductionTime = 0,
                ShopId = 1,
            },
            new Product
            {
                Id = 4,
                Name = "Bread",
                Description = "Food",
                Image = "no image",
                Price = 55,
                ProductionTime = 10,
                ShopId = 2,
            },
        };

        public void Seed(EntityTypeBuilder<Product> builder) => builder.HasData(_products);
    }
}
