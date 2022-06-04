using DAL.Entities;
using DAL.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(review => review.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(review => review.Date)
                .IsRequired();

            builder.Property(review => review.Stars)
                .IsRequired();

            builder.Property(review => review.CustomerId)
                .IsRequired();

            builder.Property(review => review.ShopId)
                .IsRequired();

            builder.HasOne(review => review.Shop)
                .WithMany(shop => shop.Reviews)
                .HasForeignKey(review => review.ShopId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(review => review.Customer)
                .WithMany(user => user.Reviews)
                .HasForeignKey(review => review.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            new ReviewSeeder().Seed(builder);
        }
    }
}
