using FluentAssertions;
using Moq;
using WMS.Application.Products.Queries;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Products.Queries
{
    public class GetProductBySkuOrNameQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetProductBySkuOrNameQueryHandler _handler;

        public GetProductBySkuOrNameQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new GetProductBySkuOrNameQueryHandler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductBySku_WhenProductExists()
        {
            // Arrange
            var product = TestDataBuilder.CreateProduct();

            _productRepositoryMock
                .Setup(x => x.GetBySkuOrNameAsync(
                    product.Sku,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var query = new GetProductBySkuOrNameQuery(product.Sku);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sku.Should().Be(product.Sku);
            result.Name.Should().Be(product.Name);
            result.Description.Should().Be(product.Description);

            _productRepositoryMock.Verify(
                x => x.GetBySkuOrNameAsync(
                    product.Sku,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductByName_WhenProductExists()
        {
            // Arrange
            var product = TestDataBuilder.CreateProduct();

            _productRepositoryMock
                .Setup(x => x.GetBySkuOrNameAsync(
                    product.Name,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var query = new GetProductBySkuOrNameQuery(product.Name);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sku.Should().Be(product.Sku);
            result.Name.Should().Be(product.Name);
            result.Description.Should().Be(product.Description);

            _productRepositoryMock.Verify(
                x => x.GetBySkuOrNameAsync(
                    product.Name,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            string productName = "null product";
            _productRepositoryMock
                .Setup(x => x.GetBySkuOrNameAsync(
                    productName,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            var query = new GetProductBySkuOrNameQuery(productName);

            // Act
            Func<Task> action = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _productRepositoryMock.Verify(
                x => x.GetBySkuOrNameAsync(
                    productName,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}