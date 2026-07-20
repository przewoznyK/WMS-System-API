using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WMS.Api.IntegrationTests.Helpers;
using WMS.Api.IntegrationTests.TestBuilders;
using WMS.Application.Stocks.Response;
using WMS.Domain.Entities;

namespace WMS.Api.IntegrationTests.Tests.Stocks
{
    public class StocksControllerGetTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public StocksControllerGetTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "FakeToken");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStocks_WhenDataExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";
            const int quantity = 10;

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var stock = new StockBuilder()
                .WithQuantity(quantity)
                .Build();

            db.Stocks.Add(stock);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/stocks");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<Stock>>();
            result.Should().NotBeNull();
            result.Should().ContainSingle();

            var stockResult = result.Single();
            stockResult.Quantity.Should().Be(quantity);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoStocksExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            // Act
            var response = await _client.GetAsync("/api/stocks");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<Stock>>();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllViewsAsync_ShouldReturnStockSummary_WhenDataExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var stock = new StockBuilder()
              .WithQuantity(TestConstants.Quantity)
              .WithProduct(TestConstants.ProductSku, TestConstants.ProductName)
              .WithLocation(TestConstants.LocationCode)
              .Build();

            db.Stocks.Add(stock);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/stocks/summary");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<StockResponse>>();

            result.Should().NotBeNull();
            result.Should().ContainSingle();

            var item = result.Single();
            item.ProductSku.Should().Be(TestConstants.ProductSku);
            item.ProductName.Should().Be(TestConstants.ProductName);
            item.LocationCode.Should().Be(TestConstants.LocationCode);
            item.Quantity.Should().Be(TestConstants.Quantity);
        }

        [Fact]
        public async Task GetAllViewsAsync_ShouldReturnEmptyList_WhenNoStocksExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            // Act
            var response = await _client.GetAsync("/api/stocks/summary");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<StockResponse>>();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllViewsByProductSkuAsync_ShouldReturnStocks_WhenProductExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var stock = new StockBuilder()
                .WithQuantity(TestConstants.Quantity)
                .WithProduct(TestConstants.ProductSku, TestConstants.ProductName)
                .WithLocation(TestConstants.LocationCode)
                .Build();

            db.Stocks.Add(stock);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"/api/stocks/by-product-sku/{TestConstants.ProductSku}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<StockResponse>>();

            result.Should().NotBeNull();
            result.Should().ContainSingle();

            var item = result.Single();
            item.ProductSku.Should().Be(TestConstants.ProductSku);
            item.ProductName.Should().Be(TestConstants.ProductName);
            item.LocationCode.Should().Be(TestConstants.LocationCode);
            item.Quantity.Should().Be(TestConstants.Quantity);
        }

        [Fact]
        public async Task GetAllViewsByProductSkuAsync_ShouldReturnEmpty_WhenProductNotFound()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            // Act
            var response = await _client.GetAsync($"/api/stocks/by-product-sku/{TestConstants.ProductSku}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<StockResponse>>();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
