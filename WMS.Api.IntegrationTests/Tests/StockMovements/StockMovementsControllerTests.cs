using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WMS.Api.IntegrationTests.Helpers;
using WMS.Api.IntegrationTests.TestBuilders;
using WMS.Application.StockMovements.Response;
using WMS.Application.StockMovements.Responses;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Tests.Common;

namespace WMS.Api.IntegrationTests.Tests.StockMovements
{
    public class StockMovementsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public StockMovementsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "FakeToken");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStockMovements_WhenDataExists()
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

            var movement = new StockMovement(
                stock,
                OperationType.Receive,
                TestConstants.Quantity,
                TestConstants.User);

            db.StockMovements.Add(movement);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/stockmovements/all");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StockMovementResponse>>();
            result.Should().NotBeNull();
            result.Should().ContainSingle();
            result.Single().QuantityChange
                .Should()
                .Be(TestConstants.Quantity);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoStockMovementsExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            // Act
            var response = await _client.GetAsync("/api/stockmovements/all");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StockMovementResponse>>();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllViewsAsync_ShouldReturnStockMovementSummary_WhenDataExists()
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

            var movement = new StockMovement(
                stock,
                OperationType.Receive,
                TestConstants.Quantity,
                TestConstants.User);

            db.StockMovements.Add(movement);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/stockmovements/summary");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StockMovementResponse>>();
            result.Should().NotBeNull();
            result.Should().ContainSingle();

            var item = result.Single();
            item.ProductSku.Should().Be(TestConstants.ProductSku);
            item.ProductName.Should().Be(TestConstants.ProductName);
            item.LocationCode.Should().Be(TestConstants.LocationCode);
            item.QuantityChange.Should().Be(TestConstants.Quantity);
        }

        [Fact]
        public async Task GetAllViewsAsync_ShouldReturnEmptyList_WhenNoStockMovementsExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            // Act
            var response = await _client.GetAsync("/api/stockmovements/summary");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StockMovementResponse>>();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetChartAsync_ShouldReturnChartData_WhenStockMovementsExist()
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

            var movement = new StockMovement(
                stock,
                OperationType.Receive,
                TestConstants.Quantity,
                TestConstants.User);

            db.StockMovements.Add(movement);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/stockmovements/chart?days=7");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StockMovementChartResponse>>();
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetChartAsync_ShouldReturnZeroValues_WhenNoStockMovementsExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            // Act
            var response = await _client.GetAsync("/api/stockmovements/chart?days=7");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<StockMovementChartResponse>>();
            result.Should().HaveCount(7);
            result.Should().AllSatisfy(item =>
            {
                item.ReceiveCount.Should().Be(0);
                item.IssueCount.Should().Be(0);
                item.TransferCount.Should().Be(0);
            });
        }
    }
}