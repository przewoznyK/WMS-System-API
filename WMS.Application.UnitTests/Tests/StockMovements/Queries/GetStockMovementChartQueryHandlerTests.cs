using FluentAssertions;
using Moq;
using WMS.Application.StockMovements.Queries;
using WMS.Domain.Data;
using WMS.Domain.Repositories;

namespace WMS.Application.UnitTests.Tests.StockMovements.Queries
{
    public class GetStockMovementChartQueryHandlerTests
    {
        private readonly Mock<IStockMovementRepository> _stockMovementRepositoryMock;
        private readonly GetStockMovementChartQueryHandler _handler;

        public GetStockMovementChartQueryHandlerTests()
        {
            _stockMovementRepositoryMock = new Mock<IStockMovementRepository>();
            _handler = new GetStockMovementChartQueryHandler(_stockMovementRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnChartData_WhenMovementsExist()
        {
            // Arrange
            var today = DateTime.UtcNow.Date;

            var chartData = new List<StockMovementChartData>
            {
                new()
                {
                    Date = today,
                    ReceiveCount = 5,
                    IssueCount = 2,
                    TransferCount = 1
                }
            };

            _stockMovementRepositoryMock
                .Setup(x => x.GetMovementChartAsync(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(chartData);

            var query = new GetStockMovementChartQuery(3);

            // Act
            var result = (await _handler.Handle(query, CancellationToken.None))
                .ToList();

            // Assert
            result.Should().HaveCount(3);

            result.Last().ReceiveCount.Should().Be(5);
            result.Last().IssueCount.Should().Be(2);
            result.Last().TransferCount.Should().Be(1);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyCounts_WhenNoMovementsExist()
        {
            _stockMovementRepositoryMock
                .Setup(x => x.GetMovementChartAsync(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<StockMovementChartData>());

            var query = new GetStockMovementChartQuery(7);

            var result = (await _handler.Handle(query, CancellationToken.None))
                .ToList();

            result.Should().HaveCount(7);
            result.Should().OnlyContain(x =>
                x.ReceiveCount == 0 &&
                x.IssueCount == 0 &&
                x.TransferCount == 0);
        }
    }
}