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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Catalogue.UnitTests
{
    public class ShopControllerTest
    {
        [Fact]
        public async Task GetPagedAsync_ValidCall()
        {
            using(var mock = AutoMock.GetLoose())
            {
                var request = new ShopParameters();
                mock.Mock<IShopService>()
                    .Setup(x => x.GetAsync(It.IsAny<ShopParameters>()))
                    .Returns(Task.Run(async () => await PagedList<ShopResponse>.ToPagedListAsync(GetSampleShops().AsQueryable(), 1, 1)));

                var controller = new ShopsController(mock.Mock<IShopService>().Object);

                var result = await controller.GetPagedAsync(request);
                result.Result.Should().BeOfType<ObjectResult>();
                mock.Mock<IShopService>().Verify(x => x.GetAsync(It.IsAny<ShopParameters>()), Times.Once);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrect()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var shop = Task.Run(() => GetSampleShops().First());
                mock.Mock<IShopService>()
                   .Setup(x => x.GetByIdAsync(1))
                   .Returns(shop);

                var controller = new ShopsController(mock.Mock<IShopService>().Object);
                var result = await controller.GetByIdAsync(1);
                result.Result.Should().BeOfType<OkObjectResult>();
            }
        }

        [Fact]
        public async Task GetByIdAsync_NotFound()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShopService>()
                   .Setup(x => x.GetByIdAsync(1))
                   .Throws<EntityNotFoundException>();

                var controller = new ShopsController(mock.Mock<IShopService>().Object);
                var result = await controller.GetByIdAsync(1);
                result.Result.Should().BeOfType<NotFoundObjectResult>();
            }
        }

        [Fact]
        public async Task InsertAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var request = new ShopRequest { Name = "name", Description = "no", AddressId = 1 };
                mock.Mock<IShopService>()
                    .Setup(x => x.InsertAsync(It.Is<ShopRequest>(s => s.Name == request.Name)));
                
                var controller = new ShopsController(mock.Mock<IShopService>().Object);
                var result = await controller.InsertAsync(request);

                mock.Mock<IShopService>().Verify(x => x.InsertAsync(It.Is<ShopRequest>(s => s.Name == request.Name)), Times.Once);
                result.Should().BeOfType<OkResult>();
            }
        }

        [Fact]
        public async Task UpdateAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var request = new ShopRequest { Name = "name", Description = "no", AddressId = 1 };
                mock.Mock<IShopService>()
                    .Setup(x => x.UpdateAsync(It.Is<ShopRequest>(s => s.Name == request.Name)));

                var controller = new ShopsController(mock.Mock<IShopService>().Object);
                var result = await controller.UpdateAsync(request);

                mock.Mock<IShopService>().Verify(x => x.UpdateAsync(It.Is<ShopRequest>(s => s.Name == request.Name)), Times.Once);
                result.Should().BeOfType<OkResult>();
            }
        }

        [Fact]
        public async Task DeleteAsync_ReturnsCorrect()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShopService>()
                   .Setup(x => x.DeleteAsync(1));

                var controller = new ShopsController(mock.Mock<IShopService>().Object);
                var result = await controller.DeleteAsync(1);
                result.Should().BeOfType<OkResult>();
            }
        }

        [Fact]
        public async Task DeleteAsync_NotFound()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShopService>()
                   .Setup(x => x.DeleteAsync(1))
                   .Throws<EntityNotFoundException>();

                var controller = new ShopsController(mock.Mock<IShopService>().Object);
                var result = await controller.DeleteAsync(1);
                result.Should().BeOfType<NotFoundObjectResult>();
            }
        }

        private IEnumerable<ShopResponse> GetSampleShops()
        {
            List<ShopResponse> shops = new()
            {
                new ShopResponse
                {
                    Id = 1,
                    Name = "Goods",
                    Description = "Nice shop",
                    Image = "no image",
                    AddressId = 1
                },
                new ShopResponse
                {
                    Id = 2,
                    Name = "Clothes",
                    Description = "Great shop",
                    Image = "no image",
                    AddressId = 2
                },
                new ShopResponse
                {
                    Id = 3,
                    Name = "Shoes",
                    Description = "Awesome shop",
                    Image = "no image",
                    AddressId = 3
                },
                new ShopResponse
                {
                    Id = 4,
                    Name = "Fruit",
                    Description = "Bad shop",
                    Image = "no image",
                    AddressId = 4
                },
            };
            return shops.AsEnumerable();
        }

    }
}