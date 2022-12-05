using Catalogue.UnitTests.Common;
using DAL.Data.Repositories;
using DAL.Entities;
using DAL.Exceptions;
using DAL.Pagination;
using DAL.Parameters;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Catalogue.UnitTests
{
    public class ShopRepositoryTest
    {
        [Fact]
        public async Task GetAsync_ReturnsAllValuesAsync()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ShopRepository(_context);
                var result = await repo.GetAsync();

                result.Should().NotBeEmpty().And.HaveCount(4);
            }
        }

        [Fact]
        public async Task GetAsync_ReturnsCorrectValueByPage()
        {
            using(var _context = new CatalogueTestBase().Context)
            {
                var repo = new ShopRepository(_context);
                var result = await repo.GetAsync(new PaginationParameters() { PageSize = 1, PageNumber = 2});

                result.Should().BeOfType<PagedList<Shop>>();
                result.TotalPages.Should().Be(4);
                result[0].Id.Should().Be(2);
            }
        }

        [Fact]
        public async Task GetAsync_FiltersByName()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ShopRepository(_context);
                var parameters = new ShopParameters { Name = "good" };
                var result = await repo.GetAsync(parameters);

                foreach(var item in result)
                {
                    item.Name.ToLower().Should().Contain(parameters.Name.ToLower());
                }
            }
        }

        [Fact]
        public async Task GetAsync_FiltersByCity()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ShopRepository(_context);
                var parameters = new ShopParameters { City = "Kyiv" };
                var result = await repo.GetAsync(parameters);

                foreach (var item in result)
                {
                    item.Address.City.ToLower().Should().Contain(parameters.City.ToLower());
                }
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectShop()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ShopRepository(_context);
                var result = await repo.GetByIdAsync(1);
                result.Id.Should().Be(1);
            }
        }

        [Fact]
        public async Task GetById_ThrowsErrorOnWrongId()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ShopRepository(_context);
                Func<Task> act = async () => await repo.GetByIdAsync(90);
                act.Should().ThrowAsync<EntityNotFoundException>();
            }
        }

        [Fact]
        public async Task InsertAsync_AddsNewShop()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ShopRepository(_context);
                var newShop = new Shop { Name = "test", AddressId = 1 };
                await repo.InsertAsync(newShop);
                _context.SaveChanges();
                var all = await repo.GetAsync();
                var last = all.ToList().MaxBy(sh => sh.Id);

                last.Name.Should().Be(newShop.Name);
                last.AddressId.Should().Be(newShop.AddressId);
            }
        }

        [Fact]
        public async Task InsertAsync_WithWrongParams()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ShopRepository(_context);
                var newShop = new Shop { AddressId = 8 };

                Func<Task> act = async () =>
                {
                    await repo.InsertAsync(newShop);
                    _context.SaveChanges();
                };
                act.Should().ThrowAsync<DbUpdateException>();
            }
        }

        [Fact]
        public async Task UpdateAsync_UpdateShop()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ShopRepository(_context);
                var newShop = new Shop { Id = 1, Name = "new name", Description = "new", Image = "new", AddressId = 1};

                await repo.UpdateAsync(newShop);
                _context.SaveChanges();

                var updated = await repo.GetByIdAsync(newShop.Id);
                updated.Name.Should().Be(newShop.Name);
                updated.Description.Should().Be(newShop.Description);
                updated.Image.Should().Be(newShop.Image);
                updated.AddressId.Should().Be(newShop.AddressId);
            }
        }

        [Fact]
        public async Task DeleteAsync_DeletesShop()
        {
            using (var _context = new CatalogueTestBase().Context)
            {
                var repo = new ShopRepository(_context);
                await repo.DeleteAsync(1);
                _context.SaveChanges();
                Func<Task> act = async () => await repo.GetByIdAsync(1);
                act.Should().ThrowAsync<EntityNotFoundException>();
            }   
        }
    }
}
