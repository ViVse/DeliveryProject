using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Catalogue.UnitTests.Common
{
    public class CatalogueTestBase
    {
        private readonly Context _context;
        public Context Context => _context;


        public CatalogueTestBase()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _context = new Context(options);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();

            _context.Dispose();
        }
    }
}
