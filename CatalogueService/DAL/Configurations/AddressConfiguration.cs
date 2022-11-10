using DAL.Entities;
using DAL.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class AddressConfiguration: IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(address => address.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(address => address.City)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(address => address.AddressLine)
                .IsRequired();

            builder.Property(address => address.Latitude)
                .IsRequired();

            builder.Property(address => address.Longitude)
                .IsRequired();

            new AddressSeeder().Seed(builder);
        }
    }
}
