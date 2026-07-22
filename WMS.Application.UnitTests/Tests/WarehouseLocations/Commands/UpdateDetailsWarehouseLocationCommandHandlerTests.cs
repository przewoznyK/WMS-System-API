using FluentAssertions;
using Moq;
using WMS.Application.WarehouseLocations.Commands;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.WarehouseLocations.Commands
{
    public class UpdateDetailsWarehouseLocationCommandHandlerTests
    {
        private readonly Mock<IWarehouseLocationRepository> _warehouseLocationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateDetailsWarehouseLocationCommandHandler _handler;

        private const string updatedCode = "AB-02-02";
        private const string updatedDescription = "Updated description";

        public UpdateDetailsWarehouseLocationCommandHandlerTests()
        {
            _warehouseLocationRepositoryMock = new Mock<IWarehouseLocationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateDetailsWarehouseLocationCommandHandler(_warehouseLocationRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenLocationDoesNotExist()
        {
            // Arrange
            var locationId = Guid.NewGuid();
            var command = new UpdateDetailsWarehouseLocationCommand(locationId, updatedCode, updatedDescription);

            _warehouseLocationRepositoryMock.Setup(x => x.GetByIdAsync(locationId, It.IsAny<CancellationToken>())).ReturnsAsync((WarehouseLocation?)null);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _warehouseLocationRepositoryMock.Verify(x => x.GetByIdAsync(locationId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldUpdateWarehouseLocation_WhenLocationExists()
        {
            // Arrange
            var location = TestDataBuilder.CreateWarehouseLocation();
            var command = new UpdateDetailsWarehouseLocationCommand(location.Id, updatedCode, updatedDescription);

            _warehouseLocationRepositoryMock.Setup(x => x.GetByIdAsync(location.Id, It.IsAny<CancellationToken>())).ReturnsAsync(location);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            location.Code.Should().Be(updatedCode);
            location.Description.Should().Be(updatedDescription);

            _warehouseLocationRepositoryMock.Verify(x => x.GetByIdAsync(location.Id, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
