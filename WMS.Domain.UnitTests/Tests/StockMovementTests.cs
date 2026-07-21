using FluentAssertions;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.Exceptions;
using WMS.Tests.Common;
using Xunit;

namespace WMS.Domain.UnitTests.Tests
{
    public class StockMovementTests
    {
        [Fact]
        public void Constructor_ShouldCreateStockMovement_WhenDataIsValid()
        {
            // Arrange
            var stock = TestDataBuilder.CreateStock();
            var operationType = OperationType.Transfer;
            var quantityChange = 10;
            var userId = "TestUser";

            // Act
            var movement = new StockMovement(stock, operationType, quantityChange, userId);

            // Assert
            movement.Id.Should().NotBeEmpty();
            movement.ProductId.Should().Be(stock.ProductId);
            movement.LocationId.Should().Be(stock.LocationId);
            movement.ProductSku.Should().Be(stock.Product.Sku);
            movement.ProductName.Should().Be(stock.Product.Name);
            movement.LocationCode.Should().Be(stock.Location.Code);
            movement.OperationType.Should().Be(operationType);
            movement.QuantityChange.Should().Be(quantityChange);
            movement.CreatedByUserId.Should().Be(userId);
            movement.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenQuantityChangeIsZero()
        {
            // Arrange
            var stock = TestDataBuilder.CreateStock();

            // Act
            Action action = () => new StockMovement(stock, OperationType.Transfer, 0, "TestUser");

            // Assert
            action.Should().Throw<WmsBusinessRuleException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowException_WhenCreatedByUserIdIsEmpty(string? createdByUserId)
        {
            // Arrange
            var stock = TestDataBuilder.CreateStock();

            // Act
            Action action = () => new StockMovement(stock, OperationType.Transfer, 10, createdByUserId!);

            // Assert
            action.Should().Throw<WmsNullOrEmptyException>();
        }
    }
}
