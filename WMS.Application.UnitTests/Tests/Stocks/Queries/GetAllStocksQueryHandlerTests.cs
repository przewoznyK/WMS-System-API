using FluentAssertions;
using Moq;
using WMS.Application.Stocks.Queries;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Stocks.Queries
{
    public class GetAllStocksQueryHandlerTests
    {
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly GetAllStocksQueryHandler _handler;

        public GetAllStocksQueryHandlerTests()
        {
            _stockRepositoryMock = new Mock<IStockRepository>();
            _handler = new GetAllStocksQueryHandler(_stockRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllStocks_WhenDataExists()
        {
            // Arrange
            var stocks = new List<Stock>
            {
                TestDataBuilder.CreateStock(quantity: 10),
                TestDataBuilder.CreateStock(quantity: 20),
                TestDataBuilder.CreateStock(quantity: 30)
            };

            _stockRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stocks);

            var query = new GetAllStocksQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(stocks);

            _stockRepositoryMock.Verify(
                x => x.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoStocksExist()
        {
            // Arrange
            var stocks = new List<Stock>();

            _stockRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stocks);

            var query = new GetAllStocksQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _stockRepositoryMock.Verify(
                x => x.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}