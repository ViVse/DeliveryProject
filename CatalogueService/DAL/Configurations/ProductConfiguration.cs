using DAL.Entities;
using DAL.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ProductConfiguration: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(product => product.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(product => product.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(product => product.Price)
                .IsRequired();

            builder.Property(product => product.ProductionTime)
                .IsRequired();

            builder.HasCheckConstraint("CK_Products_MinPrice", "[Price] > 0");
            builder.HasCheckConstraint("CK_Products_MinProductionTime", "[ProductionTime] >= 0");

            builder.HasOne(product => product.Shop)
                .WithMany(shops => shops.Products)
                .HasForeignKey(product => product.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            new ProductSeeder().Seed(builder);
        }
    }
}
