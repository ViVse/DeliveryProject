using BLL.DTO.Requests;
using BLL.DTO.Responses;
using Catalogue.IntegrationTest.Common;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Catalogue.IntegrationTest
{
    public class ProductControllerTest : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductControllerTest(TestingWebAppFactory<Program> factory) => _client = factory.CreateClient();

        [Fact]
        public async Task GetPagedAsync_ReturnsProducts()
        {
            var response = await _client.GetAsync("/api/Products");

            response.EnsureSuccessStatusCode();

            var dataRaw = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<ProductResponse>>(dataRaw);

            data.Should().NotBeNull();
            data.Count.Should().Be(4);
        }

        [Fact]
        public async Task GetPagedAsync_WrongParams()
        {
            var response = await _client.GetAsync("/api/Products?maxprice=1&minprice=5");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            response = await _client.GetAsync("/api/Products?maxproductiontime=1&minproductiontime=5");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProduct()
        {
            var response = await _client.GetAsync("/api/Products/1");

            response.EnsureSuccessStatusCode();

            var dataRaw = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ProductResponse>(dataRaw);

            data.Should().NotBeNull();
            data.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFount()
        {
            var response = await _client.GetAsync("/api/Products/10");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task InsertAsync_WithCorrectData()
        {
            string json = JsonConvert.SerializeObject(new ProductRequest() { Id = 5, Name = "name", Description = "desc", Price = 10, ProductionTime = 5, ShopId = 1 });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Products", httpContent);

            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync("/api/Products");

            var dataRaw = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<ProductResponse>>(dataRaw);

            data.Should().NotBeNull();
            data.Count.Should().Be(5);
            data.Last().Id.Should().Be(5);
        }

        [Fact]
        public async Task InsertAsync_WithWrongData()
        {
            string json = JsonConvert.SerializeObject(new ProductRequest());
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Products", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //non existed shop id
            json = JsonConvert.SerializeObject(new ProductRequest() { Id = 6, Name = "name", Description = "desc", Price = 10, ProductionTime = 5, ShopId = 10 });
            httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            response = await _client.PostAsync("/api/Products", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAsync_WithCorrectData()
        {
            var newProduct = new ProductRequest() { Id = 1, Description = "desc", Name = "test name", ShopId = 1, Price = 10, ProductionTime = 5 };
            string json = JsonConvert.SerializeObject(newProduct);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Products", httpContent);

            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync("/api/Products/1");

            var dataRaw = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ProductResponse>(dataRaw);

            data.Should().NotBeNull();
            data.Id.Should().Be(newProduct.Id);
            data.Name.Should().Be(newProduct.Name);
        }

        [Fact]
        public async Task UpdateAsync_WithWrongData()
        {
            string json = JsonConvert.SerializeObject(new ProductRequest());
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Products", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //non existed shop id
            json = JsonConvert.SerializeObject(new ProductRequest() { Id = 6, Name = "name", Description = "desc", Price = 10, ProductionTime = 5, ShopId = 10 });
            httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            response = await _client.PutAsync("/api/Products", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteAsync_CorrectId()
        {
            var response = await _client.DeleteAsync("/api/Products/1");

            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync("/api/Products/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteAsync_WrongId()
        {
            var response = await _client.DeleteAsync("/api/Products/10");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
