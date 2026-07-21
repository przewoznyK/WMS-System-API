using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WMS.Api.IntegrationTests.Helpers;
using WMS.Api.IntegrationTests.TestBuilders;
using WMS.Application.Stocks.Commands;
using WMS.Domain.Entities;
using WMS.Tests.Common;

namespace WMS.Api.IntegrationTests.Tests.Stocks
{
    public class StocksControllerReceiveTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public StocksControllerReceiveTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "FakeToken");
        }

        [Fact]
        public async Task ReceiveStock_ShouldCreateStock_WhenDataIsValid()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var product = TestDataBuilder.CreateProduct();
            var location = TestDataBuilder.CreateWarehouseLocation();

            db.Products.Add(product);
            db.WarehouseLocations.Add(location);
            await db.SaveChangesAsync();

            var command = new ReceiveStockCommand(
                TestConstants.ProductSku,
                TestConstants.LocationCode,
                TestConstants.Quantity);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/receive", command);

            // Assert
            var body = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK, body);

            var stockId = await response.Content.ReadFromJsonAsync<Guid>();
            stockId.Should().NotBeEmpty();

            var stock = await db.Stocks
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == stockId);

            stock.Should().NotBeNull();

            var savedStock = stock!;
            savedStock.Quantity.Should().Be(TestConstants.Quantity);
            savedStock.Product.Sku.Should().Be(TestConstants.ProductSku);
        }

        [Fact]
        public async Task ReceiveStock_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var location = TestDataBuilder.CreateWarehouseLocation();
            db.WarehouseLocations.Add(location);
            await db.SaveChangesAsync();

            var command = new ReceiveStockCommand(TestConstants.ProductSku, TestConstants.LocationCode, TestConstants.Quantity);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/receive", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain(nameof(Product));
            body.Should().Contain(TestConstants.ProductSku);
        }

        [Fact]
        public async Task ReceiveStock_ShouldReturnNotFound_WhenLocationDoesNotExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var product = TestDataBuilder.CreateProduct();
            db.Products.Add(product);
            await db.SaveChangesAsync();

            var command = new ReceiveStockCommand(product.Sku, TestConstants.LocationCode, TestConstants.Quantity);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/receive", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain(nameof(WarehouseLocation));
            body.Should().Contain(TestConstants.LocationCode);
        }

        [Fact]
        public async Task ReceiveStock_ShouldIncreaseQuantity_WhenStockAlreadyExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var stock = new StockBuilder()
                .WithQuantity(10)
                .WithProduct(TestConstants.ProductSku, TestConstants.ProductName)
                .WithLocation(TestConstants.LocationCode)
                .Build();

            db.Stocks.Add(stock);
            await db.SaveChangesAsync();

            var command = new ReceiveStockCommand(
                TestConstants.ProductSku,
                TestConstants.LocationCode,
                5);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/receive", command);

            var stocks = await db.Stocks.ToListAsync();
            db.ChangeTracker.Clear();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var stockId = await response.Content.ReadFromJsonAsync<Guid>();

            var updatedStock = await db.Stocks.FirstOrDefaultAsync(x => x.Id == stockId);
            updatedStock.Should().NotBeNull();
            updatedStock!.Quantity.Should().Be(15);
        }
    }
}
