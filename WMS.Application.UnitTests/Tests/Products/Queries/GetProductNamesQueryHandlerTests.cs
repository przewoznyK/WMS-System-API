using FluentAssertions;
using Moq;
using WMS.Application.Products.Queries;
using WMS.Domain.Repositories;

namespace WMS.Application.UnitTests.Tests.Products.Queries
{
    public class GetProductNamesQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetProductNamesQueryHandler _handler;

        public GetProductNamesQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new GetProductNamesQueryHandler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductsNames_WhenDataExists()
        {
            // Arrange
            var productNames = new List<string>
            {
                "Product 1",
                "Product 2",
                "Product 3"
            };

            _productRepositoryMock
                .Setup(x => x.GetNamesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(productNames);

            var query = new GetProductNamesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(productNames);

            _productRepositoryMock.Verify(x => x.GetNamesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var productNames = new List<string>();

            _productRepositoryMock
                .Setup(x => x.GetNamesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(productNames);

            var query = new GetProductNamesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _productRepositoryMock.Verify(x => x.GetNamesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}