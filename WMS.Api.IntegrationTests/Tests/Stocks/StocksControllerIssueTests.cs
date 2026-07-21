using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WMS.Api.IntegrationTests.Helpers;
using WMS.Api.IntegrationTests.TestBuilders;
using WMS.Application.Stocks.Commands;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Tests.Common;

namespace WMS.Api.IntegrationTests.Tests.Stocks
{
    public class StocksControllerIssueTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public StocksControllerIssueTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "FakeToken");
        }

        [Fact]
        public async Task IssueStock_ShouldDecreaseQuantity_WhenStockExists()
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
            var command = new IssueStockCommand(
                TestConstants.ProductSku,
                TestConstants.LocationCode,
                TestConstants.IssueQuantity,
                TestConstants.ReferenceNumber,
                IssueType.SalesOrder);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/issue", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var movementId = await response.Content.ReadFromJsonAsync<Guid>();
            movementId.Should().NotBeEmpty();

            db.ChangeTracker.Clear();

            var updatedStock = await db.Stocks.FirstOrDefaultAsync(x => x.Id == stock.Id);
            updatedStock.Should().NotBeNull();
            updatedStock!.Quantity.Should().Be(7);
        }

        [Fact]
        public async Task IssueStock_ShouldReturnNotFound_WhenStockDoesNotExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            var command = new IssueStockCommand(
                TestConstants.ProductSku,
                TestConstants.LocationCode,
                TestConstants.Quantity,
                TestConstants.ReferenceNumber,
                IssueType.SalesOrder);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/issue", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain(nameof(Stock));
            body.Should().Contain(TestConstants.ProductSku);
        }

        [Fact]
        public async Task IssueStock_ShouldReturnBadRequest_WhenQuantityExceedsAvailableStock()
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

            var command = new IssueStockCommand(
                TestConstants.ProductSku,
                TestConstants.LocationCode,
                15,
                TestConstants.ReferenceNumber,
                IssueType.SalesOrder);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/issue", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            body!["details"].Should().Be("Can't take more quantity than is in the stock.");
        }

        [Fact]
        public async Task IssueStock_ShouldCreateStockMovement_WhenStockIsIssued()
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

            var command = new IssueStockCommand(
                TestConstants.ProductSku,
                TestConstants.LocationCode,
                TestConstants.IssueQuantity,
                TestConstants.ReferenceNumber,
                IssueType.SalesOrder);

            // Act
            var response = await _client.PostAsJsonAsync("/api/stocks/issue", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var movementId = await response.Content.ReadFromJsonAsync<Guid>();
            movementId.Should().NotBeEmpty();

            db.ChangeTracker.Clear();

            var movement = await db.StockMovements.FirstOrDefaultAsync(x => x.Id == movementId);
            movement.Should().NotBeNull();
            movement!.OperationType.Should().Be(OperationType.Issue);
            movement.QuantityChange.Should().Be(-3);
            movement.ReferenceNumber.Should().Be(TestConstants.ReferenceNumber);
            movement.IssueType.Should().Be(IssueType.SalesOrder);
        }
    }
}
