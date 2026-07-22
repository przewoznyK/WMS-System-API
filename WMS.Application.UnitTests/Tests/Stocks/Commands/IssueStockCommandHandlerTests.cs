using FluentAssertions;
using Moq;
using WMS.Application.Authentication.Interfaces;
using WMS.Application.Stocks.Commands;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Stocks.Commands
{
    public class IssueStockCommandHandlerTests
    {
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly Mock<IStockMovementRepository> _stockMovementRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly IssueStockCommandHandler _handler;

        public IssueStockCommandHandlerTests()
        {
            _stockRepositoryMock = new Mock<IStockRepository>();
            _stockMovementRepositoryMock = new Mock<IStockMovementRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userContextMock = new Mock<IUserContext>();

            _handler = new IssueStockCommandHandler(
                _stockRepositoryMock.Object,
                _stockMovementRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var command = new IssueStockCommand(TestConstants.ProductSku, TestConstants.LocationCode, 3, TestConstants.ReferenceNumber, IssueType.SalesOrder);

            _userContextMock
                .Setup(x => x.UserId)
                .Returns(string.Empty);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsBusinessRuleException>();

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);

            _stockMovementRepositoryMock.Verify(x => x.Add(It.IsAny<StockMovement>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenStockDoesNotExist()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var command = new IssueStockCommand(TestConstants.ProductSku, TestConstants.LocationCode, 3, TestConstants.ReferenceNumber, IssueType.SalesOrder);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(
                    command.ProductSku,
                    command.LocationCode,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Stock?)null);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    command.ProductSku,
                    command.LocationCode,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockMovementRepositoryMock.Verify(x => x.Add(It.IsAny<StockMovement>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDecreaseQuantity_WhenStockExists()
        {
            // Arrange
            _userContextMock
                .Setup(x => x.UserId)
                .Returns("TestUser");

            var stock = TestDataBuilder.CreateStock(quantity: 10);
            var command = new IssueStockCommand(stock.Product.Sku, stock.Location.Code, 3, "REF-001", IssueType.SalesOrder);

            _stockRepositoryMock
                .Setup(x => x.GetByProductSkuAndLocationCodeAsync(command.ProductSku, command.LocationCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(stock);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            stock.Quantity.Should().Be(7);

            _stockRepositoryMock.Verify(
                x => x.GetByProductSkuAndLocationCodeAsync(
                    command.ProductSku,
                    command.LocationCode,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _stockMovementRepositoryMock.Verify(
                x => x.Add(It.Is<StockMovement>(m =>
                    m.OperationType == OperationType.Issue &&
                    m.QuantityChange == -3 &&
                    m.CreatedByUserId == "TestUser" &&
                    m.IssueType == IssueType.SalesOrder &&
                    m.ReferenceNumber == "REF-001")),
                Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
