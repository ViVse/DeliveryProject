using DAL.Entities;
using DAL.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class DeliveryManConfiguration : IEntityTypeConfiguration<DeliveryMan>
    {
        public void Configure(EntityTypeBuilder<DeliveryMan> builder)
        {
            builder.Property(d => d.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(d => d.FirstName)
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(d => d.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(d => d.Phone)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength(true);

            new DeliveryManSeeder().Seed(builder);
        }
    }
}
