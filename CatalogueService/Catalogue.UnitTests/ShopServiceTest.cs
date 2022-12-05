using Autofac.Extras.Moq;
using AutoMapper;
using BLL.Configurations;
using BLL.DTO.Requests;
using BLL.DTO.Responses;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Catalogue.UnitTests
{
    public class ShopServiceTest
    {
        private IMapper mapper = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfile())).CreateMapper();

        [Fact]
        public async Task GetAsync_ReturnsAllShopsAsync()
        {
            using(var mock = AutoMock.GetLoose())
            {
                mock.Mock<IUnitOfWork>()
                    .Setup(x => x.ShopRepository.GetAsync())
                    .Returns(Task.Run(() => GetSampleShops()));
                var unit = mock.Create<IUnitOfWork>();
                var service = new ShopService(unit, mapper);
                var result = await service.GetAsync();
                result.Should().HaveCount(4);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrect()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var shop = Task.Run(() => GetSampleShops().First());
                mock.Mock<IUnitOfWork>()
                    .Setup(x => x.ShopRepository.GetByIdAsync(1))
                    .Returns(shop);
                var unit = mock.Create<IUnitOfWork>();
                var service = new ShopService(unit, mapper);
                var result = await service.GetByIdAsync(1);
                result.Should().BeOfType<ShopResponse>();
            }
        }

        [Fact]
        public async Task InsertAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var request = new ShopRequest { Name = "name", Description = "no", AddressId = 1 };
                var shop = mapper.Map<Shop>(request);
                mock.Mock<IUnitOfWork>()
                    .Setup(x => x.ShopRepository.InsertAsync(It.Is<Shop>(s => s.Name == request.Name)));
                var unit = mock.Create<IUnitOfWork>();
                var service = new ShopService(unit, mapper);

                await service.InsertAsync(request);
                
                mock.Mock<IUnitOfWork>().Verify(x => x.ShopRepository.InsertAsync(It.Is<Shop>(s => s.Name == request.Name)), Times.Once);
            }
        }

        [Fact]
        public async Task UpdateAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var request = new ShopRequest { Id = 1, Name = "name", Description = "no", AddressId = 1 };
                var shop = mapper.Map<Shop>(request);
                mock.Mock<IUnitOfWork>()
                    .Setup(x => x.ShopRepository.UpdateAsync(It.Is<Shop>(s => s.Id == request.Id)));
                var unit = mock.Create<IUnitOfWork>();
                var service = new ShopService(unit, mapper);

                await service.UpdateAsync(request);

                mock.Mock<IUnitOfWork>().Verify(x => x.ShopRepository.UpdateAsync(It.Is<Shop>(s => s.Name == request.Name)), Times.Once);
            }
        }

        [Fact]
        public async Task DeleteAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IUnitOfWork>()
                    .Setup(x => x.ShopRepository.DeleteAsync(1));
                var unit = mock.Create<IUnitOfWork>();
                var service = new ShopService(unit, mapper);

                await service.DeleteAsync(1);

                mock.Mock<IUnitOfWork>().Verify(x => x.ShopRepository.DeleteAsync(1), Times.Once);
            }
        }

        private IEnumerable<Shop> GetSampleShops()
        {
            List<Shop> shops = new()
            {
                new Shop
                {
                    Id = 1,
                    Name = "Goods",
                    Description = "Nice shop",
                    Image = "no image",
                    AddressId = 1
                },
                new Shop
                {
                    Id = 2,
                    Name = "Clothes",
                    Description = "Great shop",
                    Image = "no image",
                    AddressId = 2
                },
                new Shop
                {
                    Id = 3,
                    Name = "Shoes",
                    Description = "Awesome shop",
                    Image = "no image",
                    AddressId = 3
                },
                new Shop
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
