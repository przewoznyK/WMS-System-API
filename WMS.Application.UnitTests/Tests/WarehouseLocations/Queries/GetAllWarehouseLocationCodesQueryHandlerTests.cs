using FluentAssertions;
using Moq;
using WMS.Application.WarehouseLocations.Queries;
using WMS.Domain.Repositories;

namespace WMS.Application.UnitTests.Tests.WarehouseLocations.Queries
{
    public class GetAllWarehouseLocationCodesQueryHandlerTests
    {
        private readonly Mock<IWarehouseLocationRepository> _warehouseLocationRepository;
        private readonly GetAllWarehouseLocationCodesQueryHandler _handler;

        public GetAllWarehouseLocationCodesQueryHandlerTests()
        {
            _warehouseLocationRepository = new Mock<IWarehouseLocationRepository>();
            _handler = new GetAllWarehouseLocationCodesQueryHandler(_warehouseLocationRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnLocationCodes_WhenLocationsExist()
        {
            // Arrange
            var locations = new List<string>
            {
                "AA-01-01",
                "AA-01-02",
                "BB-02-01"
            };

            _warehouseLocationRepository
                .Setup(x => x.GetLocationsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(locations);

            var query = new GetAllWarehouseLocationCodesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(locations);

            _warehouseLocationRepository.Verify(x => x.GetLocationsAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoLocationsExist()
        {
            // Arrange
            _warehouseLocationRepository
                .Setup(x => x.GetLocationsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<string>());

            var query = new GetAllWarehouseLocationCodesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _warehouseLocationRepository.Verify(x => x.GetLocationsAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}