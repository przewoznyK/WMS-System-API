using FluentAssertions;
using Moq;
using WMS.Application.Products.Queries;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Products.Queries
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new GetAllProductsQueryHandler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProducts_WhenDataExists()
        {
            // Arrange
            var products = new List<Product>
            {
                TestDataBuilder.CreateProduct("SKU-001", "Product 1"),
                TestDataBuilder.CreateProduct("SKU-002", "Product 2"),
                TestDataBuilder.CreateProduct("SKU-003", "Product 3")
            };

            _productRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query = new GetAllProductsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Select(x => x.Id)
                  .Should()
                  .BeEquivalentTo(products.Select(x => x.Id));

            _productRepositoryMock.Verify(
                x => x.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var products = new List<Product>();

            _productRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query = new GetAllProductsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _productRepositoryMock.Verify(
                x => x.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}