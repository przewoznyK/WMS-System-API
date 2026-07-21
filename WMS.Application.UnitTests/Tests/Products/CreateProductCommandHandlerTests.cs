using FluentAssertions;
using Moq;
using WMS.Application.Products.Commands;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Products
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateProductCommandHandler(_productRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct_WhenSkuIsAvailable()
        {
            // Arrange
            var command = new CreateProductCommand(TestConstants.ProductSku, TestConstants.ProductName, TestConstants.ProductDescription);

            _productRepositoryMock
                .Setup(x => x.ExistsBySkuAsync(command.Sku, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();

            _productRepositoryMock.Verify(x => x.ExistsBySkuAsync(command.Sku, It.IsAny<CancellationToken>()), Times.Once);

            _productRepositoryMock.Verify(
                x => x.Add(It.Is<Product>(p =>
                    p.Sku == command.Sku &&
                    p.Name == command.Name &&
                    p.Description == command.Description)),
                Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSkuAlreadyExists()
        {
            // Arrange
            var command = new CreateProductCommand(TestConstants.ProductSku, TestConstants.ProductName, TestConstants.ProductDescription);

            _productRepositoryMock
                .Setup(x => x.ExistsBySkuAsync(command.Sku, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsAlreadyExistsException>();

            _productRepositoryMock.Verify(x => x.ExistsBySkuAsync(command.Sku, It.IsAny<CancellationToken>()), Times.Once);
            _productRepositoryMock.Verify(x => x.Add(It.IsAny<Product>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}