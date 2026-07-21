using FluentAssertions;
using Moq;
using WMS.Application.WarehouseLocations.Commands;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.WarehouseLocations
{
    public class DeleteWarehouseLocationCommandHandlerTests
    {
        private readonly Mock<IWarehouseLocationRepository> _warehouseLocationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DeleteWarehouseLocationCommandHandler _handler;

        public DeleteWarehouseLocationCommandHandlerTests()
        {
            _warehouseLocationRepositoryMock = new Mock<IWarehouseLocationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeleteWarehouseLocationCommandHandler(_warehouseLocationRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteWarehouseLocation_WhenLocationExists()
        {
            // Arrange
            var location = TestDataBuilder.CreateWarehouseLocation();
            var command = new DeleteWarehouseLocationCommand(location.Id);

            _warehouseLocationRepositoryMock
                .Setup(x => x.GetByIdAsync(location.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(location);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _warehouseLocationRepositoryMock.Verify(x => x.GetByIdAsync(location.Id, It.IsAny<CancellationToken>()), Times.Once);
            _warehouseLocationRepositoryMock.Verify(x => x.Delete(location), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenLocationDoesNotExist()
        {
            // Arrange
            var locationId = Guid.NewGuid();
            var command = new DeleteWarehouseLocationCommand(locationId);
            _warehouseLocationRepositoryMock
                .Setup(x => x.GetByIdAsync(locationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((WarehouseLocation?)null);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _warehouseLocationRepositoryMock.Verify(x => x.GetByIdAsync(locationId, It.IsAny<CancellationToken>()), Times.Once);
            _warehouseLocationRepositoryMock.Verify(x => x.Delete(It.IsAny<WarehouseLocation>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}