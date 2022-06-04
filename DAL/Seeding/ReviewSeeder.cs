using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Seeding
{
    public class ReviewSeeder : ISeeder<Review>
    {
        private static readonly List<Review> reviews = new()
        {
            new Review
            {
                Id = 1,
                Text = "nice shop",
                Date = DateTime.Now,
                ShopId = 1,
                CustomerId = "3b333929-f974-444e-a8d3-68f50a0459c0"
            },
            new Review
            {
                Id = 2,
                Text = "good service",
                Date = DateTime.Now,
                ShopId = 2,
                CustomerId = "3b333929-f974-444e-a8d3-68f50a0459c0"
            }
        };

        public void Seed(EntityTypeBuilder<Review> builder) => builder.HasData(reviews);
    }
}
