using FluentAssertions;
using Moq;
using WMS.Application.Dashboard.Queries;
using WMS.Application.Products.Queries;
using WMS.Domain.Repositories;

namespace WMS.Application.UnitTests.Tests.Dashboard.Queries
{
    public class GetDashboardSummaryQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly Mock<IWarehouseLocationRepository> _locationRepositoryMock;
        private readonly Mock<IStockMovementRepository> _movementRepositoryMock;
        private readonly GetDashboardSummaryQueryHandler _handler;

        public GetDashboardSummaryQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _stockRepositoryMock = new Mock<IStockRepository>();
            _locationRepositoryMock = new Mock<IWarehouseLocationRepository>();
            _movementRepositoryMock = new Mock<IStockMovementRepository>();

            _handler = new GetDashboardSummaryQueryHandler(
                _productRepositoryMock.Object,
                _stockRepositoryMock.Object,
                _locationRepositoryMock.Object,
                _movementRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnDashboardSummary_WhenDataExists()
        {
            // Arrange
            _productRepositoryMock
                .Setup(x => x.GetCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(50);

            _locationRepositoryMock
                .Setup(x => x.GetCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(10);

            _stockRepositoryMock
                .Setup(x => x.GetSumQuantityAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(500);

            _movementRepositoryMock
                .Setup(x => x.GetCountTodayAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(25);

            _stockRepositoryMock
                .Setup(x => x.GetLowStockCountAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(7);

            // Act
            var result = await _handler.Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProductCount.Should().Be(50);
            result.LocationCount.Should().Be(10);
            result.StockQuantity.Should().Be(500);
            result.TodayMovements.Should().Be(25);
            result.LowStockCount.Should().Be(7);

            _productRepositoryMock.Verify(x => x.GetCountAsync(It.IsAny<CancellationToken>()), Times.Once);
            _locationRepositoryMock.Verify(x => x.GetCountAsync(It.IsAny<CancellationToken>()), Times.Once);
            _stockRepositoryMock.Verify(x => x.GetSumQuantityAsync(It.IsAny<CancellationToken>()), Times.Once);
            _movementRepositoryMock.Verify(x => x.GetCountTodayAsync(It.IsAny<CancellationToken>()), Times.Once);
            
            _stockRepositoryMock.Verify(
                x => x.GetLowStockCountAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptySummary_WhenNoDataExists()
        {
            // Arrange
            _productRepositoryMock
                .Setup(x => x.GetCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _locationRepositoryMock
                .Setup(x => x.GetCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _stockRepositoryMock
                .Setup(x => x.GetSumQuantityAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _movementRepositoryMock
                .Setup(x => x.GetCountTodayAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _stockRepositoryMock
                .Setup(x => x.GetLowStockCountAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProductCount.Should().Be(0);
            result.LocationCount.Should().Be(0);
            result.StockQuantity.Should().Be(0);
            result.TodayMovements.Should().Be(0);
            result.LowStockCount.Should().Be(0);
        }
    }
}