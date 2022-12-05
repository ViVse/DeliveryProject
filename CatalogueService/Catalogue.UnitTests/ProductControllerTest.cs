using Autofac.Extras.Moq;
using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Interfaces.Services;
using DAL.Entities;
using DAL.Exceptions;
using DAL.Pagination;
using DAL.Parameters;
using EFController.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Catalogue.UnitTests
{
    public class ProductControllerTest
    {
        [Fact]
        public async Task GetPagedAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var request = new ProductParameters();
                mock.Mock<IShopService>();
                mock.Mock<IProductService>()
                    .Setup(x => x.GetAsync(It.IsAny<ProductParameters>()))
                    .Returns(Task.Run(async () => await PagedList<ProductResponse>.ToPagedListAsync(GetSampleProducts().AsQueryable(), 1, 1)));

                var controller = new ProductsController(mock.Mock<IProductService>().Object, mock.Mock<IShopService>().Object);

                var result = await controller.GetPagedAsync(request);
                result.Result.Should().BeOfType<ObjectResult>();
                mock.Mock<IProductService>().Verify(x => x.GetAsync(It.IsAny<ProductParameters>()), Times.Once);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrect()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var product = Task.Run(() => GetSampleProducts().First());
                mock.Mock<IShopService>();
                mock.Mock<IProductService>()
                    .Setup(x => x.GetByIdAsync(1))
                    .Returns(product);

                var controller = new ProductsController(mock.Mock<IProductService>().Object, mock.Mock<IShopService>().Object);
                var result = await controller.GetByIdAsync(1);
                result.Result.Should().BeOfType<OkObjectResult>();
            }
        }

        [Fact]
        public async Task GetByIdAsync_NotFound()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShopService>();
                mock.Mock<IProductService>()
                   .Setup(x => x.GetByIdAsync(1))
                   .Throws<EntityNotFoundException>();

                var controller = new ProductsController(mock.Mock<IProductService>().Object, mock.Mock<IShopService>().Object);
                var result = await controller.GetByIdAsync(1);
                result.Result.Should().BeOfType<NotFoundObjectResult>();
            }
        }

        [Fact]
        public async Task InsertAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var request = new ProductRequest { Id = 1, Name = "name", Price = 10, ProductionTime = 5, ShopId = 1 };
                mock.Mock<IShopService>()
                    .Setup(x => x.GetByIdAsync(request.ShopId))
                    .Returns(Task.Run(() => new ShopResponse()));
                mock.Mock<IProductService>()
                    .Setup(x => x.InsertAsync(It.Is<ProductRequest>(s => s.Name == request.Name)));

                var controller = new ProductsController(mock.Mock<IProductService>().Object, mock.Mock<IShopService>().Object);
                var result = await controller.InsertAsync(request);

                mock.Mock<IProductService>().Verify(x => x.InsertAsync(It.Is<ProductRequest>(s => s.Name == request.Name)), Times.Once);
                result.Should().BeOfType<OkResult>();
            }
        }

        [Fact]
        public async Task UpdateAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var request = new ProductRequest { Id = 1, Name = "name", Price = 10, ProductionTime = 5, ShopId = 1 };
                mock.Mock<IShopService>()
                    .Setup(x => x.GetByIdAsync(request.ShopId))
                    .Returns(Task.Run(() => new ShopResponse()));
                mock.Mock<IProductService>()
                    .Setup(x => x.UpdateAsync(It.Is<ProductRequest>(s => s.Name == request.Name)));

                var controller = new ProductsController(mock.Mock<IProductService>().Object, mock.Mock<IShopService>().Object);
                var result = await controller.UpdateAsync(request);

                mock.Mock<IProductService>().Verify(x => x.UpdateAsync(It.Is<ProductRequest>(s => s.Name == request.Name)), Times.Once);
                result.Should().BeOfType<OkResult>();
            }
        }

        [Fact]
        public async Task DeleteAsync_ReturnsCorrect()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShopService>();
                mock.Mock<IProductService>()
                   .Setup(x => x.DeleteAsync(1));

                var controller = new ProductsController(mock.Mock<IProductService>().Object, mock.Mock<IShopService>().Object);
                var result = await controller.DeleteAsync(1);
                result.Should().BeOfType<OkResult>();
            }
        }

        [Fact]
        public async Task DeleteAsync_NotFound()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShopService>();
                mock.Mock<IProductService>()
                   .Setup(x => x.DeleteAsync(1))
                   .Throws<EntityNotFoundException>();

                var controller = new ProductsController(mock.Mock<IProductService>().Object, mock.Mock<IShopService>().Object);
                var result = await controller.DeleteAsync(1);
                result.Should().BeOfType<NotFoundObjectResult>();
            }
        }

        private IEnumerable<ProductResponse> GetSampleProducts()
        {
            List<ProductResponse> products = new()
            {
                new ProductResponse
                {
                    Id = 1,
                    Name = "Banana",
                    Description = "Fruit",
                    Image = "no image",
                    Price = 55,
                    ProductionTime = 10,
                    ShopId = 3,
                },
                new ProductResponse
                {
                    Id = 2,
                    Name = "Pants",
                    Description = "Levis",
                    Image = "no image",
                    Price = 155,
                    ProductionTime = 0,
                    ShopId = 2,
                },
                new ProductResponse
                {
                    Id = 3,
                    Name = "Ledder",
                    Description = "Useful",
                    Image = "no image",
                    Price = 35,
                    ProductionTime = 0,
                    ShopId = 1,
                },
                new ProductResponse
                {
                    Id = 4,
                    Name = "Bread",
                    Description = "Food",
                    Image = "no image",
                    Price = 55,
                    ProductionTime = 10,
                    ShopId = 2,
                },
            };
            return products.AsEnumerable();
        }
    }
}
