using FluentAssertions;
using Moq;
using WMS.Application.Stocks.Queries;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Stocks.Queries
{
    public class GetAllStocksViewsByProductSkuQueryHandlerTests
    {
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly GetAllStocksViewsByProductSkuQueryHandler _handler;

        public GetAllStocksViewsByProductSkuQueryHandlerTests()
        {
            _stockRepositoryMock = new Mock<IStockRepository>();
            _handler = new GetAllStocksViewsByProductSkuQueryHandler(_stockRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnStocksBySku_WhenStocksExist()
        {
            // Arrange
            var product = TestDataBuilder.CreateProduct("SKU-001", "Product 1");
            var location = TestDataBuilder.CreateWarehouseLocation("AA-01-01");

            var stocks = new List<Stock>
            {
                new Stock(product, location, 10)
            };

            _stockRepositoryMock
                .Setup(x => x.GetAllByProductSkuAsync(
                    product.Sku,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(stocks);

            var query = new GetAllStocksViewsByProductSkuQuery(product.Sku);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();

            var stockResponse = result.Single();

            stockResponse.ProductSku.Should().Be(product.Sku);
            stockResponse.ProductName.Should().Be(product.Name);
            stockResponse.LocationCode.Should().Be(location.Code);
            stockResponse.Quantity.Should().Be(10);

            _stockRepositoryMock.Verify(
                x => x.GetAllByProductSkuAsync(
                    product.Sku,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoStocksExist()
        {
            // Arrange
            string sku = "SKU-NOT-FOUND";

            _stockRepositoryMock
                .Setup(x => x.GetAllByProductSkuAsync(
                    sku,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Stock>());

            var query = new GetAllStocksViewsByProductSkuQuery(sku);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _stockRepositoryMock.Verify(
                x => x.GetAllByProductSkuAsync(
                    sku,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}