using System.Net.Http.Json;
using System.Net;
using WMS.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using WMS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WMS.Api.IntegrationTests.TestBuilders;
using WMS.Application.Stocks.Response;
using WMS.Application.Stocks.Commands;

namespace WMS.Api.IntegrationTests
{
    public class StocksControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public StocksControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Test", "FakeToken");
        }
    

        [Fact]
        public async Task GetAllAsync_ShouldReturnStocks_WhenDataExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";
            const int quantity = 10;

            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            var stock = new StockBuilder()
                .WithQuantity(quantity)
                .Build();

            db.Stocks.Add(stock);

            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/stocks");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<Stock>>();

            result.Should().NotBeNull();
            result.Should().ContainSingle();

            result!.Single().Quantity.Should().Be(quantity);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoStocksExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";
            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            // Act
            var response = await _client.GetAsync("/api/stocks");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<Stock>>();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData("Manager", HttpStatusCode.OK)]
        [InlineData("Worker", HttpStatusCode.OK)]
        [InlineData("Guest", HttpStatusCode.Forbidden)]
        public async Task GetAllAsync_ShouldReturnExpectedStatusCode_ForRole(string role, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            TestAuthHandler.Role = role;

            // Act
            var response = await _client.GetAsync("/api/stocks");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task GetAllViewsAsync_ShouldReturnStockSummary_WhenDataExists()
        {
            // Arrange
            const string sku = "SKU-TEST";
            const string productName = "Test Product";
            const string locationCode = "XX-01-01";
            const int quantity = 10;

            TestAuthHandler.Role = "Manager";

            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            var stock = new StockBuilder()
              .WithQuantity(quantity)
              .WithProduct(sku, productName)
              .WithLocation(locationCode)
              .Build();

            db.Stocks.Add(stock);

            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/stocks/summary");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StockResponse>>();
            Console.WriteLine(result);

            result.Should().NotBeNull();
            result.Should().ContainSingle();

            var item = result.Single();

            item.ProductSku.Should().Be(sku);
            item.ProductName.Should().Be(productName);
            item.LocationCode.Should().Be(locationCode);
            item.Quantity.Should().Be(quantity);
        }

        [Fact]
        public async Task GetAllViewsAsync_ShouldReturnEmptyList_WhenNoStocksExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            // Act
            var response = await _client.GetAsync("/api/stocks/summary");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StockResponse>>();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllViewsByProductSkuAsync_ShouldReturnStocks_WhenProductExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            const string sku = "SKU-TEST";
            const string productName = "Test Product";
            const string locationCode = "AA-01-01";
            const int quantity = 10;

            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            var stock = new StockBuilder()
                .WithQuantity(quantity)
                .WithProduct(sku, productName)
                .WithLocation(locationCode)
                .Build();

            db.Stocks.Add(stock);

            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"/api/stocks/by-product-sku/{sku}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StockResponse>>();

            result.Should().NotBeNull();
            result.Should().ContainSingle();

            var item = result.Single();

            item.ProductSku.Should().Be(sku);
            item.ProductName.Should().Be(productName);
            item.LocationCode.Should().Be(locationCode);
            item.Quantity.Should().Be(quantity);
        }

        [Fact]
        public async Task GetAllViewsByProductSkuAsync_ShouldReturnEmpty_WhenProductNotFound()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            const string sku = "NOT-EXISTS";

            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            // Act
            var response = await _client.GetAsync($"/api/stocks/by-product-sku/{sku}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StockResponse>>();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ReceiveStock_ShouldCreateStock_WhenDataIsValid()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            const string sku = "SKU-TEST";
            const string productName = "Test Product";
            const string locationCode = "AA-01-01";
            const int quantity = 10;

            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            var product = new Product(sku, productName);
            var location = new WarehouseLocation(locationCode);

            db.Products.Add(product);
            db.WarehouseLocations.Add(location);

            await db.SaveChangesAsync();

            var command = new ReceiveStockCommand(
                sku,
                locationCode,
                quantity);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/receive", command);

            // Assert
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine(body);

            response.StatusCode.Should().Be(HttpStatusCode.OK, body);

            var stockId = await response.Content.ReadFromJsonAsync<Guid>();

            stockId.Should().NotBeEmpty();

            var stock = await db.Stocks
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == stockId);

            stock.Should().NotBeNull();

            var savedStock = stock!;
            
            savedStock.Quantity.Should().Be(quantity);
            savedStock.Product.Sku.Should().Be(sku);
        }

        [Fact]
        public async Task ReceiveStock_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arange
            TestAuthHandler.Role = "Manager";

            const string sku = "NOT_EXISTS";
            const string locationCode = "AA-01-01";
            const int quantity = 10;

            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            var location = new WarehouseLocation(locationCode);

            db.WarehouseLocations.Add(location);

            await db.SaveChangesAsync();

            var command = new ReceiveStockCommand(sku, locationCode, quantity);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/receive", command);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await response.Content.ReadAsStringAsync();

            body.Should().Contain("Product");
            body.Should().Contain(sku);
        }

        [Fact]
        public async Task ReceiveStock_ShouldReturnNotFound_WhenLocationDoesNotExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";


            const string sku = "SKU-TEST";
            const string productName = "Test Product";
            const string locationCode = "AA-99-99";
            const int quantity = 10;

            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();
            var product = new Product(sku, productName);

            db.Products.Add(product);
            
            await db.SaveChangesAsync();

            var command = new ReceiveStockCommand(sku, locationCode, quantity);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/receive", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await response.Content.ReadAsStringAsync();

            body.Should().Contain("WarehouseLocation");
            body.Should().Contain(locationCode);
        }
    }
}