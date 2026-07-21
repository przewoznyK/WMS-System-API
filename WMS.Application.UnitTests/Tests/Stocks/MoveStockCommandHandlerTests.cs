using FluentAssertions;
using Moq;
using WMS.Application.Authentication.Interfaces;
using WMS.Application.Products.Commands;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Stocks
{
    public class MoveStockCommandHandlerTests
    {
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly Mock<IStockMovementRepository> _stockMovementRepositoryMock;
        private readonly Mock<IWarehouseLocationRepository> _warehouseLocationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly MoveStockCommandHandler _handler;

        private const int _sourceStockQuantity = 15;
        private const int _destinationStockQuantity = 10;
        private const int _moveStockQuantity = 5;

        public MoveStockCommandHandlerTests()
        {
            _stockRepositoryMock = new Mock<IStockRepository>();
            _stockMovementRepositoryMock = new Mock<IStockMovementRepository>();
            _warehouseLocationRepositoryMock = new Mock<IWarehouseLocationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userContextMock = new Mock<IUserContext>();

            _handler = new MoveStockCommandHandler(
                _stockRepositoryMock.Object,
                _stockMovementRepositoryMock.Object,
                _warehouseLocationRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenMoveQuantityIsNotPositive()
        {
            // Arrange
            var sourceLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var destinationLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.SecondLocationCode);
            var product = TestDataBuilder.CreateProduct();
            var command = new MoveStockCommand(product.Sku, sourceLocation.Code, destinationLocation.Code, 0);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsBusinessRuleException>();

            _stockRepositoryMock.Verify(x => x.GetByProductSkuAndLocationCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<StockMovement>>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Delete(It.IsAny<Stock>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns(string.Empty);

            var sourceLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var destinationLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.SecondLocationCode);
            var product = TestDataBuilder.CreateProduct();
            var command = new MoveStockCommand(product.Sku, sourceLocation.Code, destinationLocation.Code, _moveStockQuantity);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsBusinessRuleException>();

            _stockRepositoryMock.Verify(x => x.GetByProductSkuAndLocationCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<StockMovement>>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Delete(It.IsAny<Stock>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenLocationsAreTheSame()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var sourceLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var product = TestDataBuilder.CreateProduct();
            var command = new MoveStockCommand(product.Sku, sourceLocation.Code, sourceLocation.Code, _moveStockQuantity);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsBusinessRuleException>();

            _stockRepositoryMock.Verify(x => x.GetByProductSkuAndLocationCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<StockMovement>>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Delete(It.IsAny<Stock>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSourceStockDoesNotExist()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var sourceLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var destinationLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.SecondLocationCode);
            var product = TestDataBuilder.CreateProduct();
            var command = new MoveStockCommand(product.Sku, sourceLocation.Code, destinationLocation.Code, _moveStockQuantity);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    command.ProductSku,
                    command.SourceLocationCode,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Stock?)null);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    sourceLocation.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    destinationLocation.Code,
                    It.IsAny<CancellationToken>()),
                Times.Never);

            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<StockMovement>>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Delete(It.IsAny<Stock>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSourceStockIsEmpty()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var sourceLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var destinationLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.SecondLocationCode);
            var product = TestDataBuilder.CreateProduct();
            var sourceStock = new Stock(product, sourceLocation, 0);
            var command = new MoveStockCommand(sourceStock.Product.Sku, sourceLocation.Code, destinationLocation.Code, _moveStockQuantity);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    command.ProductSku,
                    command.SourceLocationCode,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceStock);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsBusinessRuleException>();

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    sourceStock.Location.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    destinationLocation.Code,
                    It.IsAny<CancellationToken>()),
                Times.Never);

            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<StockMovement>>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Delete(It.IsAny<Stock>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenQuantityExceedsSourceStock()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var sourceLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var destinationLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.SecondLocationCode);
            var product = TestDataBuilder.CreateProduct();
            var sourceStock = new Stock(product, sourceLocation, _sourceStockQuantity);
            var command = new MoveStockCommand(sourceStock.Product.Sku, sourceLocation.Code, destinationLocation.Code, _sourceStockQuantity + _sourceStockQuantity);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    command.ProductSku,
                    command.SourceLocationCode,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceStock);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsBusinessRuleException>();

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    sourceStock.Location.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    destinationLocation.Code,
                    It.IsAny<CancellationToken>()),
                Times.Never);

            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<StockMovement>>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Delete(It.IsAny<Stock>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldMoveStock_WhenSourceAndDestinationStockExist()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var sourceLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var destinationLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.SecondLocationCode);
            var product = TestDataBuilder.CreateProduct();
            var sourceStock = new Stock(product, sourceLocation, _sourceStockQuantity);
            var destinationStock = new Stock(product, destinationLocation, _destinationStockQuantity);
            var command = new MoveStockCommand(product.Sku, sourceLocation.Code, destinationLocation.Code, _moveStockQuantity);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    sourceLocation.Code,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceStock);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    destinationLocation.Code,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationStock);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            sourceStock.Quantity.Should().Be(_sourceStockQuantity - _moveStockQuantity);
            destinationStock.Quantity.Should().Be(_destinationStockQuantity + _moveStockQuantity);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    sourceLocation.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    destinationLocation.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(destinationLocation.Code, It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);

            _stockMovementRepositoryMock.Verify(
                x => x.AddRangeAsync(
                    It.Is<IEnumerable<StockMovement>>(m =>
                        m.Any(x =>
                            x.LocationId == sourceStock.Location.Id &&
                            x.QuantityChange == -_moveStockQuantity &&
                            x.OperationType == OperationType.Transfer &&
                            x.CreatedByUserId == "TestUser") &&
                        m.Any(x =>
                            x.LocationId == destinationStock.Location.Id &&
                            x.QuantityChange == _moveStockQuantity &&
                            x.OperationType == OperationType.Transfer &&
                            x.CreatedByUserId == "TestUser")),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockRepositoryMock.Verify(x => x.Delete(It.IsAny<Stock>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCreateDestinationStock_WhenDestinationStockDoesNotExist()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var sourceLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var destinationLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.SecondLocationCode);
            var product = TestDataBuilder.CreateProduct();
            var sourceStock = new Stock(product, sourceLocation, _sourceStockQuantity);
            var command = new MoveStockCommand(product.Sku, sourceLocation.Code, destinationLocation.Code, _moveStockQuantity);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    sourceLocation.Code,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceStock);

            _warehouseLocationRepositoryMock
                .Setup(x => x.GetByCodeAsync(
                    destinationLocation.Code,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationLocation);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            sourceStock.Quantity.Should().Be(_sourceStockQuantity - _moveStockQuantity);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    sourceLocation.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    destinationLocation.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(destinationLocation.Code, It.IsAny<CancellationToken>()), Times.Once);

            _stockRepositoryMock.Verify(
                x => x.Add(It.Is<Stock>(s =>
                    s.Location == destinationLocation &&
                    s.Quantity == _moveStockQuantity)),
                Times.Once);

            _stockMovementRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<StockMovement>>(), It.IsAny<CancellationToken>()), Times.Once);
            _stockRepositoryMock.Verify(x => x.Delete(It.IsAny<Stock>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenDestinationLocationDoesNotExist()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var sourceLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var destinationLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.SecondLocationCode);
            var product = TestDataBuilder.CreateProduct();
            var sourceStock = new Stock(product, sourceLocation, _sourceStockQuantity);
            var command = new MoveStockCommand(sourceStock.Product.Sku, sourceLocation.Code, destinationLocation.Code, _moveStockQuantity);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    command.ProductSku,
                    command.SourceLocationCode,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceStock);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    command.ProductSku,
                    command.DestinationLocationCode,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Stock?)null);

            _warehouseLocationRepositoryMock
                .Setup(x => x.GetByCodeAsync(
                    command.DestinationLocationCode,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((WarehouseLocation?)null);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    sourceStock.Product.Sku,
                    sourceStock.Location.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    sourceStock.Product.Sku,
                    TestConstants.SecondLocationCode,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(TestConstants.SecondLocationCode, It.IsAny<CancellationToken>()), Times.Once);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);
            _stockMovementRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<StockMovement>>(), It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Delete(It.IsAny<Stock>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDeleteSourceStock_WhenQuantityReachesZero()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var sourceLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.LocationCode);
            var destinationLocation = TestDataBuilder.CreateWarehouseLocation(TestConstants.SecondLocationCode);
            var product = TestDataBuilder.CreateProduct();
            var sourceStock = new Stock(product, sourceLocation, _sourceStockQuantity);
            var destinationStock = new Stock(product, destinationLocation, _destinationStockQuantity);
            var command = new MoveStockCommand(product.Sku, sourceLocation.Code, destinationLocation.Code, _sourceStockQuantity);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    sourceStock.Location.Code,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceStock);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    destinationLocation.Code,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationStock);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            sourceStock.Quantity.Should().Be(0);
            destinationStock.Quantity.Should().Be(_destinationStockQuantity + _sourceStockQuantity);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    sourceStock.Location.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    product.Sku,
                    destinationLocation.Code,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _warehouseLocationRepositoryMock.Verify(x => x.GetByCodeAsync(destinationLocation.Code, It.IsAny<CancellationToken>()), Times.Never);
            _stockRepositoryMock.Verify(x => x.Add(It.IsAny<Stock>()), Times.Never);

            _stockMovementRepositoryMock.Verify(
                x => x.AddRangeAsync(
                    It.Is<IEnumerable<StockMovement>>(m =>
                        m.Any(x =>
                            x.LocationId == sourceStock.Location.Id &&
                            x.QuantityChange == -_sourceStockQuantity &&
                            x.OperationType == OperationType.Transfer &&
                            x.CreatedByUserId == "TestUser") &&
                        m.Any(x =>
                            x.LocationId == destinationStock.Location.Id &&
                            x.QuantityChange == _sourceStockQuantity &&
                            x.OperationType == OperationType.Transfer &&
                            x.CreatedByUserId == "TestUser")),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockRepositoryMock.Verify(x => x.Delete(sourceStock), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
