using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class ContextFactory: IDesignTimeDbContextFactory<Context> {
        public Context CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Delivery_Entity"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Delivery_Entity"));
            return new(optionsBuilder.Options);
        }
    }
}
