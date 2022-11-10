using DAL.Entities;
using DAL.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ShopConfiguration: IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.Property(shop => shop.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
                

            builder.Property(shop => shop.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(shop => shop.Address)
                .WithMany(address => address.Shops)
                .HasForeignKey(shop => shop.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            new ShopSeeder().Seed(builder);
        }
    }
}
