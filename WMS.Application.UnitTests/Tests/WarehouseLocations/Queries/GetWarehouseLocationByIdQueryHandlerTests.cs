using FluentAssertions;
using Moq;
using WMS.Application.WarehouseLocations.Queries;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.WarehouseLocations.Queries
{
    public class GetWarehouseLocationByIdQueryHandlerTests
    {
        private readonly Mock<IWarehouseLocationRepository> _warehouseLocationRepositoryMock;
        private readonly GetWarehouseLocationByIdQueryHandler _handler;

        public GetWarehouseLocationByIdQueryHandlerTests()
        {
            _warehouseLocationRepositoryMock = new Mock<IWarehouseLocationRepository>();
            _handler = new GetWarehouseLocationByIdQueryHandler(_warehouseLocationRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnWarehouseLocation_WhenLocationExists()
        {
            // Arrange
            var location = TestDataBuilder.CreateWarehouseLocation();
            var query = new GetWarehouseLocationByIdQuery(location.Id);

            _warehouseLocationRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    location.Id,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(location);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(location);
            result.Id.Should().Be(location.Id);
            result.Code.Should().Be(location.Code);
            result.Description.Should().Be(location.Description);

            _warehouseLocationRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    location.Id,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenLocationDoesNotExist()
        {
            // Arrange
            var locationId = Guid.NewGuid();
            var query = new GetWarehouseLocationByIdQuery(locationId);

            _warehouseLocationRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    locationId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((WarehouseLocation?)null);

            // Act
            Func<Task> action = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<WmsNotFoundException>();

            _warehouseLocationRepositoryMock.Verify(
                x => x.GetByIdAsync(
                    locationId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}