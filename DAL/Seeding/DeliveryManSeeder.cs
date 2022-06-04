using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Seeding
{
    public class DeliveryManSeeder : ISeeder<DeliveryMan>
    {
        private static readonly List<DeliveryMan> workers = new()
        {
            new DeliveryMan
            {
                Id = 1,
                FirstName = "Alex",
                LastName = "Bob",
                Phone = "0990502341"
            },
            new DeliveryMan
            {
                Id = 2,
                FirstName = "John",
                LastName = "Fisher",
                Phone = "0950507641"
            }
        };

        public void Seed(EntityTypeBuilder<DeliveryMan> builder) => builder.HasData(workers);
    }
}
