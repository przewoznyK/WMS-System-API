using FluentAssertions;
using Moq;
using WMS.Application.Products.Queries;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Products.Queries
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new GetProductByIdQueryHandler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = TestDataBuilder.CreateProduct();

            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    product.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);


            var query = new GetProductByIdQuery(product.Id);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(product.Id);
            result.Sku.Should().Be(product.Sku);
            result.Name.Should().Be(product.Name);
            result.Description.Should().Be(product.Description);

            _productRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    product.Id,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    productId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            var query = new GetProductByIdQuery(productId);

            // Act
            Func<Task> action = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _productRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    productId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}