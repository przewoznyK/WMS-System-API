using FluentAssertions;
using Moq;
using WMS.Application.Authentication.Interfaces;
using WMS.Application.Products.Commands;
using WMS.Application.Stocks.Commands;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Stocks
{
    public class ReceiveStockCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IWarehouseLocationRepository> _warehouseLocationRepositoryMock;
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly Mock<IStockMovementRepository> _stockMovementRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly ReceiveStockCommandHandler _handler;

        public ReceiveStockCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _warehouseLocationRepositoryMock = new Mock<IWarehouseLocationRepository>();
            _stockRepositoryMock = new Mock<IStockRepository>();
            _stockMovementRepositoryMock = new Mock<IStockMovementRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userContextMock = new Mock<IUserContext>();

            _handler = new ReceiveStockCommandHandler(
                _productRepositoryMock.Object,
                _warehouseLocationRepositoryMock.Object,
                _stockRepositoryMock.Object,
                _stockMovementRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenReceiveQuantityIsNotPositive()
        {
            // Arrange
            var location = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var product = TestDataBuilder.CreateProduct();
            var command = new ReceiveStockCommand(product.Sku, location.Code, 0);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsBusinessRuleException>();

            _productRepositoryMock.Verify(x => x.GetBySkuAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.GetByProductIdAndLocationAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.Add(It.IsAny<StockMovement>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns(string.Empty);

            var location = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var product = TestDataBuilder.CreateProduct();
            var command = new ReceiveStockCommand(product.Sku, location.Code, TestConstants.Quantity);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsBusinessRuleException>();

            _productRepositoryMock.Verify(x => x.GetBySkuAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.GetByProductIdAndLocationAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.Add(It.IsAny<StockMovement>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var location = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var product = TestDataBuilder.CreateProduct();
            var command = new ReceiveStockCommand(product.Sku, location.Code, TestConstants.Quantity);

            _productRepositoryMock
                .Setup(x => x.GetBySkuAsync(
                    command.ProductSku,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _productRepositoryMock.Verify(x => x.GetBySkuAsync(product.Sku, It.IsAny<CancellationToken>()), Times.Once);
            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.GetByProductIdAndLocationAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.Add(It.IsAny<StockMovement>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenLocationDoesNotExist()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var location = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var product = TestDataBuilder.CreateProduct();
            var command = new ReceiveStockCommand(product.Sku, location.Code, TestConstants.Quantity);

            _productRepositoryMock
                .Setup(x => x.GetBySkuAsync(product.Sku, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _warehouseLocationRepositoryMock
                .Setup(x => x.GetByCodeAsync(command.LocationCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync((WarehouseLocation?)null);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _productRepositoryMock.Verify(x => x.GetBySkuAsync(product.Sku, It.IsAny<CancellationToken>()), Times.Once);
            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(location.Code, It.IsAny<CancellationToken>()), Times.Once);
            _stockRepositoryMock.Verify(x => x.GetByProductIdAndLocationAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.Add(It.IsAny<StockMovement>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldCreateStock_WhenStockDoesNotExist()
        {
            // Arrange
            var product = TestDataBuilder.CreateProduct();
            var location = TestDataBuilder.CreateWarehouseLocation();
            var command = new ReceiveStockCommand(product.Sku, location.Code, TestConstants.Quantity);

            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            _productRepositoryMock
                .Setup(x => x.GetBySkuAsync(product.Sku, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _warehouseLocationRepositoryMock
                .Setup(x => x.GetByCodeAsync(location.Code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(location);

            Stock? createdStock = null;

            _stockRepositoryMock
                .Setup(x => x.Add(It.IsAny<Stock>()))
                .Callback<Stock>(s => createdStock = s)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            result.Should().Be(createdStock!.Id);

            _productRepositoryMock.Verify(x => x.GetBySkuAsync(product.Sku, It.IsAny<CancellationToken>()), Times.Once);
            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(location.Code, It.IsAny<CancellationToken>()), Times.Once);
            _stockRepositoryMock.Verify(x => x.GetByProductIdAndLocationAsync(product.Id, location.Id, It.IsAny<CancellationToken>()), Times.Once);

            _stockRepositoryMock.Verify(
                 x => x.Add(It.Is<Stock>(s =>
                     s.Product == product &&
                     s.Location == location &&
                     s.Quantity == TestConstants.Quantity)),
                 Times.Once);

            _stockMovementRepositoryMock.Verify(
                x => x.Add(It.Is<StockMovement>(m =>
                    m.QuantityChange == TestConstants.Quantity &&
                    m.CreatedByUserId == "TestUser" &&
                    m.OperationType == OperationType.Receive &&
                    m.LocationId == location.Id &&
                    m.ProductId == product.Id)),
                Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldIncreaseQuantity_WhenStockAlreadyExists()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            const int initialQuantity = 10;
            const int quantityChange = 5;

            var product = TestDataBuilder.CreateProduct();
            var location = TestDataBuilder.CreateWarehouseLocation();
            var stock = new Stock(product, location, initialQuantity);
            var command = new ReceiveStockCommand(product.Sku, location.Code, quantityChange);

            _productRepositoryMock
                .Setup(x => x.GetBySkuAsync(product.Sku, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _warehouseLocationRepositoryMock
                .Setup(x => x.GetByCodeAsync(location.Code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(location);

            _stockRepositoryMock
                .Setup(x => x.GetByProductIdAndLocationAsync(product.Id, location.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(stock);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            stock.Quantity.Should().Be(quantityChange + initialQuantity);

            _productRepositoryMock.Verify(
                x => x.GetBySkuAsync(
                    product.Sku,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _warehouseLocationRepositoryMock.Verify(
                x => x.GetByCodeAsync(
                    location.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockRepositoryMock.Verify(
                x => x.GetByProductIdAndLocationAsync(
                    product.Id,
                    location.Id,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockMovementRepositoryMock.Verify(
                x => x.Add(It.Is<StockMovement>(m =>
                    m.QuantityChange == command.Quantity &&
                    m.OperationType == OperationType.Receive &&
                    m.CreatedByUserId == "TestUser")),
                Times.Once);

            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
