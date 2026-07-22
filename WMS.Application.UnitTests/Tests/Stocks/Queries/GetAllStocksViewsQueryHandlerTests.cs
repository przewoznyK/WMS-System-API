using FluentAssertions;
using Moq;
using WMS.Application.Stocks.Queries;
using WMS.Application.Stocks.Response;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Stocks.Queries
{
    public class GetAllStocksViewsQueryHandlerTests
    {
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly GetAllStocksViewsQueryHandler _handler;

        public GetAllStocksViewsQueryHandlerTests()
        {
            _stockRepositoryMock = new Mock<IStockRepository>();
            _handler = new GetAllStocksViewsQueryHandler(_stockRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnStockViews_WhenStocksExist()
        {
            // Arrange
            var product1 = TestDataBuilder.CreateProduct("SKU-001", "Product 1");
            var location1 = TestDataBuilder.CreateWarehouseLocation("AA-01-01");

            var product2 = TestDataBuilder.CreateProduct("SKU-002", "Product 2");
            var location2 = TestDataBuilder.CreateWarehouseLocation("BB-02-02");

            var stocks = new List<Stock>
            {
                new Stock(product1, location1, 10),
                new Stock(product2, location2, 20)
            };

            _stockRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stocks);

            var query = new GetAllStocksViewsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result.Should().BeEquivalentTo(new[]
            {
                new StockResponse
                {
                    ProductSku = product1.Sku,
                    ProductName = product1.Name,
                    LocationCode = location1.Code,
                    Quantity = 10
                },
                new StockResponse
                {
                    ProductSku = product2.Sku,
                    ProductName = product2.Name,
                    LocationCode = location2.Code,
                    Quantity = 20
                }
            });

            _stockRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoStocksExist()
        {
            // Arrange
            _stockRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Stock>());

            var query = new GetAllStocksViewsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _stockRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}