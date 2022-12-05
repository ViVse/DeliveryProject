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
    public class ProductServiceTest
    {
        private IMapper mapper = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfile())).CreateMapper();

        [Fact]
        public async Task GetAsync_ReturnsAllProducts()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IUnitOfWork>()
                    .Setup(x => x.ProductRepository.GetAsync())
                    .Returns(Task.Run(() => GetSampleProducts()));
                var unit = mock.Create<IUnitOfWork>();
                var service = new ProductService(unit, mapper);
                var result = await service.GetAsync();
                result.Should().HaveCount(4);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrect()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var product = Task.Run(() => GetSampleProducts().First());
                mock.Mock<IUnitOfWork>()
                    .Setup(x => x.ProductRepository.GetByIdAsync(1))
                    .Returns(product);
                var unit = mock.Create<IUnitOfWork>();
                var service = new ProductService(unit, mapper);
                var result = await service.GetByIdAsync(1);
                result.Should().BeOfType<ProductResponse>();
            }
        }

        [Fact]
        public async Task InsertAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var request = new ProductRequest { Name = "name", Description = "no", Price = 10, ProductionTime = 10 };
                var product = mapper.Map<Product>(request);
                mock.Mock<IUnitOfWork>()
                    .Setup(x => x.ProductRepository.InsertAsync(It.Is<Product>(s => s.Name == request.Name)));
                var unit = mock.Create<IUnitOfWork>();
                var service = new ProductService(unit, mapper);

                await service.InsertAsync(request);

                mock.Mock<IUnitOfWork>().Verify(x => x.ProductRepository.InsertAsync(It.Is<Product>(s => s.Name == request.Name)), Times.Once);
            }
        }

        [Fact]
        public async Task UpdateAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var request = new ProductRequest { Id = 1, Name = "name", Description = "no", Price = 10, ProductionTime = 10 };
                var product = mapper.Map<Product>(request);
                mock.Mock<IUnitOfWork>()
                    .Setup(x => x.ProductRepository.UpdateAsync(It.Is<Product>(s => s.Id == request.Id)));
                var unit = mock.Create<IUnitOfWork>();
                var service = new ProductService(unit, mapper);

                await service.UpdateAsync(request);

                mock.Mock<IUnitOfWork>().Verify(x => x.ProductRepository.UpdateAsync(It.Is<Product>(s => s.Name == request.Name)), Times.Once);
            }
        }

        [Fact]
        public async Task DeleteAsync_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IUnitOfWork>()
                    .Setup(x => x.ProductRepository.DeleteAsync(1));
                var unit = mock.Create<IUnitOfWork>();
                var service = new ProductService(unit, mapper);

                await service.DeleteAsync(1);

                mock.Mock<IUnitOfWork>().Verify(x => x.ProductRepository.DeleteAsync(1), Times.Once);
            }
        }

        private IEnumerable<Product> GetSampleProducts()
        {
            List<Product> products = new()
            {
                new Product
                {
                    Id = 1,
                    Name = "Banana",
                    Description = "Fruit",
                    Image = "no image",
                    Price = 55,
                    ProductionTime = 10,
                    ShopId = 3,
                },
                new Product
                {
                    Id = 2,
                    Name = "Pants",
                    Description = "Levis",
                    Image = "no image",
                    Price = 155,
                    ProductionTime = 0,
                    ShopId = 2,
                },
                new Product
                {
                    Id = 3,
                    Name = "Ledder",
                    Description = "Useful",
                    Image = "no image",
                    Price = 35,
                    ProductionTime = 0,
                    ShopId = 1,
                },
                new Product
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
