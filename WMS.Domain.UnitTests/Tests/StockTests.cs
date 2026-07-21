using FluentAssertions;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Tests.Common;
using Xunit;

namespace WMS.Domain.UnitTests.Tests
{
    public class StockTests
    {
        #region Constructor

        [Fact]
        public void Constructor_ShouldCreateStock_WhenDataIsValid()
        {
            // Arrange
            var product = TestDataBuilder.CreateProduct();
            var location = TestDataBuilder.CreateWarehouseLocation();

            // Act
            var stock = new Stock(product, location, 10);

            // Assert
            stock.Id.Should().NotBeEmpty();
            stock.Product.Should().Be(product);
            stock.Location.Should().Be(location);
            stock.ProductId.Should().Be(product.Id);
            stock.LocationId.Should().Be(location.Id);
            stock.Quantity.Should().Be(10);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenProductIsNull()
        {
            // Arrange
            var location = TestDataBuilder.CreateWarehouseLocation();

            // Act
            Action action = () => new Stock(null!, location, 10);

            // Assert
            action.Should().Throw<WmsNullOrEmptyException>();
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenLocationIsNull()
        {
            // Arrange
            var product = TestDataBuilder.CreateProduct();

            // Act
            Action action = () => new Stock(product, null!, 10);

            // Assert
            action.Should().Throw<WmsNullOrEmptyException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Constructor_ShouldThrowException_WhenQuantityIsNegative(int quantity)
        {
            // Arrange
            var product = TestDataBuilder.CreateProduct();
            var location = TestDataBuilder.CreateWarehouseLocation();

            // Act
            Action action = () => new Stock(product, location, quantity);

            // Assert
            action.Should().Throw<WmsBusinessRuleException>();
        }

        #endregion

        #region IncreaseQuantity

        [Fact]
        public void IncreaseQuantity_ShouldAddQuantity_WhenValueIsPositive()
        {
            // Arrange
            var stock = TestDataBuilder.CreateStock(quantity: 10);

            // Act
            stock.IncreaseQuantity(5);

            // Assert
            stock.Quantity.Should().Be(15);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void IncreaseQuantity_ShouldThrowException_WhenValueIsNotPositive(int value)
        {
            // Arrange
            var stock = TestDataBuilder.CreateStock();

            // Act
            Action action = () => stock.IncreaseQuantity(value);

            // Assert
            action.Should().Throw<WmsBusinessRuleException>();
        }
        #endregion

        #region DecreaseQuantity
        [Fact]
        public void DecreaseQuantity_ShouldSubtractQuantity_WhenEnoughStockExists()
        {
            // Arrange
            var stock = TestDataBuilder.CreateStock(quantity: 10);

            // Act
            stock.DecreaseQuantity(4);

            // Assert
            stock.Quantity.Should().Be(6);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void DecreaseQuantity_ShouldThrowException_WhenValueIsNotPositive(int value)
        {
            // Arrange
            var stock = TestDataBuilder.CreateStock(quantity: 10);

            // Act
            Action action = () => stock.DecreaseQuantity(value);

            // Assert
            action.Should().Throw<WmsBusinessRuleException>();
        }

        [Fact]
        public void DecreaseQuantity_ShouldThrowException_WhenQuantityIsTooLarge()
        {
            // Arrange
            var stock = TestDataBuilder.CreateStock(quantity: 10);

            // Act
            Action action = () => stock.DecreaseQuantity(20);

            // Assert
            action.Should().Throw<WmsBusinessRuleException>();
        }

        #endregion
    }
}