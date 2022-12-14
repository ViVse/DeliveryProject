using BLL.DTO.Requests;
using BLL.DTO.Responses;
using Catalogue.IntegrationTest.Common;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Catalogue.IntegrationTest
{
    public class ShopControllerTest: IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;

        public ShopControllerTest(TestingWebAppFactory<Program> factory) => _client = factory.CreateClient();

        [Fact]
        public async Task GetPagedAsync_ReturnsPosts()
        {
            var response = await _client.GetAsync("/api/Shops");

            response.EnsureSuccessStatusCode();

            var dataRaw = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<ShopResponse>>(dataRaw);

            data.Should().NotBeNull();
            data.Count.Should().Be(4);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPost()
        {
            var response = await _client.GetAsync("/api/Shops/1");

            response.EnsureSuccessStatusCode();

            var dataRaw = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ShopResponse>(dataRaw);

            data.Should().NotBeNull();
            data.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFount()
        {
            var response = await _client.GetAsync("/api/Shops/10");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task InsertAsync_WithCorrectData()
        {
            string json = JsonConvert.SerializeObject(new ShopRequest() { Id = 5, Description="desc", Name="name", AddressId = 1});
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Shops", httpContent);

            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync("/api/Shops");

            var dataRaw = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<ShopResponse>>(dataRaw);

            data.Should().NotBeNull();
            data.Count.Should().Be(5);
            data.Last().Id.Should().Be(5);
        }

        [Fact]
        public async Task InsertAsync_WithWrongData()
        {
            string json = JsonConvert.SerializeObject(new ShopRequest());
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Shops", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAsync_WithCorrectData()
        {
            var newShop = new ShopRequest() { Id = 1, Description = "desc", Name = "test name", AddressId = 1 };
            string json = JsonConvert.SerializeObject(newShop);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Shops", httpContent);

            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync("/api/Shops/1");

            var dataRaw = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ShopResponse>(dataRaw);

            data.Should().NotBeNull();
            data.Id.Should().Be(newShop.Id);
            data.Name.Should().Be(newShop.Name);
        }

        [Fact]
        public async Task UpdateAsync_WithWrongData()
        {
            string json = JsonConvert.SerializeObject(new ShopRequest());
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Shops", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteAsync_CorrectId()
        {
            var response = await _client.DeleteAsync("/api/Shops/1");

            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync("/api/Shops/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteAsync_WrongId()
        {
            var response = await _client.DeleteAsync("/api/Shops/10");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}