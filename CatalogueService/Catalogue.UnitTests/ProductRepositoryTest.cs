using Catalogue.UnitTests.Common;
using DAL.Data.Repositories;
using DAL.Entities;
using DAL.Exceptions;
using DAL.Pagination;
using DAL.Parameters;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Catalogue.UnitTests
{
    public class ProductRepositoryTest
    {
        [Fact]
        public async Task GetAsync_ReturnsAllValuesAsync()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ProductRepository(_context);
                var result = await repo.GetAsync();

                result.Should().NotBeEmpty().And.HaveCount(4);
            }
        }

        [Fact]
        public async Task GetAsync_ReturnsCorrectValueByPage()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ProductRepository(_context);
                var result = await repo.GetAsync(new PaginationParameters() { PageSize = 1, PageNumber = 2 });

                result.Should().BeOfType<PagedList<Product>>();
                result.TotalPages.Should().Be(4);
                result[0].Id.Should().Be(2);
            }
        }

        [Fact]
        public async Task GetAsync_FiltersByName()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ProductRepository(_context);
                var parameters = new ProductParameters { Name = "ban" };
                var result = await repo.GetAsync(parameters);

                foreach (var item in result)
                {
                    item.Name.ToLower().Should().Contain(parameters.Name.ToLower());
                }
            }
        }

        //TODO
        //Check other filters
        [Fact]
        public async Task GetAsync_FiltersByParams()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ProductRepository(_context);
                var parameters = new ProductParameters { MaxPrice = 20, MinPrice = 1, MaxProductionTime = 10, MinProductionTime = 2 };
                var result = await repo.GetAsync(parameters);

                foreach (var item in result)
                {
                    item.Price.Should().Match(x => x < parameters.MaxPrice && x > parameters.MinPrice);
                    item.ProductionTime.Should().Match(x => x < parameters.MaxProductionTime && x > parameters.MinProductionTime);
                }
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectProduct()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ProductRepository(_context);
                var result = await repo.GetByIdAsync(1);
                result.Id.Should().Be(1);
            }
        }

        [Fact]
        public async Task GetById_ThrowsErrorOnWrongId()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ProductRepository(_context);
                Func<Task> act = async () => await repo.GetByIdAsync(90);
                act.Should().ThrowAsync<EntityNotFoundException>();
            }
        }

        [Fact]
        public async Task InsertAsync_AddsNewProduct()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ProductRepository(_context);
                var newProduct = new Product { Name = "test", Price = 10, ProductionTime = 2, ShopId = 1 };
                await repo.InsertAsync(newProduct);
                _context.SaveChanges();
                var all = await repo.GetAsync();
                var last = all.ToList().MaxBy(sh => sh.Id);

                last.Name.Should().Be(newProduct.Name);
                last.Price.Should().Be(newProduct.Price);
                last.ProductionTime.Should().Be(newProduct.ProductionTime);
                last.ShopId.Should().Be(newProduct.ShopId);
            }
        }

        [Fact]
        public async Task InsertAsync_WithWrongParams()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ProductRepository(_context);
                var newProduct = new Product { Price = 8 };

                Func<Task> act = async () =>
                {
                    await repo.InsertAsync(newProduct);
                    _context.SaveChanges();
                };
                act.Should().ThrowAsync<DbUpdateException>();
            }
        }

        [Fact]
        public async Task UpdateAsync_UpdateProduct()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ProductRepository(_context);
                var newProduct = new Product
                {
                    Id = 1,
                    Name = "new Banana",
                    Description = "new Fruit",
                    Image = "no image",
                    Price = 55,
                    ProductionTime = 10,
                    ShopId = 3,
                };

                await repo.UpdateAsync(newProduct);
                _context.SaveChanges();

                var updated = await repo.GetByIdAsync(newProduct.Id);
                updated.Name.Should().Be(newProduct.Name);
                updated.Price.Should().Be(newProduct.Price);
                updated.ProductionTime.Should().Be(newProduct.ProductionTime);
                updated.ShopId.Should().Be(newProduct.ShopId);
            }
        }

        [Fact]
        public async Task DeleteAsync_DeletesProduct()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ProductRepository(_context);
                await repo.DeleteAsync(1);
                _context.SaveChanges();
                Func<Task> act = async () => await repo.GetByIdAsync(1);
                act.Should().ThrowAsync<EntityNotFoundException>();
            }
        }
    }
}
