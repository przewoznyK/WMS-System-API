using FluentAssertions;
using Moq;
using WMS.Application.Products.Queries;
using WMS.Application.Stocks.Queries;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Application.UnitTests.Tests.Products.Queries
{
    public class GetLowStockProductsQueryHandlerTests
    {
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly GetLowStockProductsQueryHandler _handler;

        private const int _threshold = 5;

        public GetLowStockProductsQueryHandlerTests()
        {
            _stockRepositoryMock = new Mock<IStockRepository>();
            _handler = new GetLowStockProductsQueryHandler(_stockRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnLowStockProducts_WhenDataExists()
        {
            // Arrange
            var product1 = new LowStockProduct
            {
                Sku = "SKU-001",
                Name = "Product 1",
                Quantity = 5
            };

            var product2 = new LowStockProduct
            {
                Sku = "SKU-002",
                Name = "Product 2",
                Quantity = 2
            };

            var lowStockProducts = new List<LowStockProduct>
            {
                product1,
                product2
            };

            _stockRepositoryMock
                .Setup(x => x.GetLowStockAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(lowStockProducts);

            var query = new GetLowStockProductsQuery(_threshold);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result.Should().Contain(x =>
                x.ProductSku == product1.Sku &&
                x.ProductName == product1.Name &&
                x.Quantity == product1.Quantity);

            _stockRepositoryMock.Verify(x => x.GetLowStockAsync(_threshold, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoLowStockProductsExist()
        {
            // Arrange
            _stockRepositoryMock
                .Setup(x => x.GetLowStockAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LowStockProduct>());

            var query = new GetLowStockProductsQuery(_threshold);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _stockRepositoryMock.Verify(
                x => x.GetLowStockAsync(
                    _threshold,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}