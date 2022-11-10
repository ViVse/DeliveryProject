using GraphQL.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Shop> Shops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasMany(p => p.Reviews)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder
                .Entity<Shop>()
                .HasMany(p => p.Reviews)
                .WithOne(p => p.Shop)
                .HasForeignKey(p => p.ShopId);

            modelBuilder
                .Entity<Review>()
                .HasOne(p => p.User)
                .WithMany(p => p.Reviews)
                .HasForeignKey(p => p.UserId);
        }
    }
}
