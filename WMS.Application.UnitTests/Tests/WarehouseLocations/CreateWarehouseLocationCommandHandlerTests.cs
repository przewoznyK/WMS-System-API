using FluentAssertions;
using Moq;
using WMS.Application.WarehouseLocations.Commands;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.WarehouseLocations
{
    public class CreateWarehouseLocationCommandHandlerTests
    {
        private readonly Mock<IWarehouseLocationRepository> _warehouseLocationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateWarehouseLocationCommandHandler _handler;

        public CreateWarehouseLocationCommandHandlerTests()
        {
            _warehouseLocationRepositoryMock = new Mock<IWarehouseLocationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateWarehouseLocationCommandHandler(_warehouseLocationRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateWarehouseLocation_WhenCodeIsAvailable()
        {
            // Arrange
            var command = new CreateWarehouseLocationCommand(TestConstants.LocationCode, TestConstants.LocationDescription);

            _warehouseLocationRepositoryMock
                .Setup(x => x.ExistsByCodeAsync(command.Code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();

            _warehouseLocationRepositoryMock.Verify(x => x.ExistsByCodeAsync(command.Code, It.IsAny<CancellationToken>()), Times.Once);

            _warehouseLocationRepositoryMock.Verify(
                x => x.Add(It.Is<WarehouseLocation>(l =>
                    l.Code == command.Code &&
                    l.Description == command.Description)),
                Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCodeAlreadyExists()
        {
            // Arrange
            var command = new CreateWarehouseLocationCommand(TestConstants.LocationCode, TestConstants.LocationDescription);

            _warehouseLocationRepositoryMock
                .Setup(x => x.ExistsByCodeAsync(command.Code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsAlreadyExistsException>();

            _warehouseLocationRepositoryMock.Verify(x => x.ExistsByCodeAsync(command.Code, It.IsAny<CancellationToken>()), Times.Once);
            _warehouseLocationRepositoryMock.Verify(x => x.Add(It.IsAny<WarehouseLocation>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}