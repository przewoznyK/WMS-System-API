using FluentAssertions;
using Moq;
using System;
using WMS.Application.Products.Commands;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Products.Commands
{
    public class UpdateDetailsProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateDetailsProductCommandHandler _handler;

        private const string updatedName = "Updated Name";
        private const string updatedDescription = "Updated Description";

        public UpdateDetailsProductCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateDetailsProductCommandHandler(_productRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new UpdateDetailsProductCommand(productId, updatedName, updatedDescription);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act 
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _productRepositoryMock.Verify(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var product = TestDataBuilder.CreateProduct();
            var command = new UpdateDetailsProductCommand(product.Id, updatedName, updatedDescription);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act 
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            product.Name.Should().Be(updatedName);
            product.Description.Should().Be(updatedDescription);

            _productRepositoryMock.Verify(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}