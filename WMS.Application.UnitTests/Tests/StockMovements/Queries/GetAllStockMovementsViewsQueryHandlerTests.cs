using FluentAssertions;
using Moq;
using WMS.Application.Authentication.Interfaces;
using WMS.Application.StockMovements.Queries;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.StockMovements.Queries
{
    public class GetAllStockMovementsViewsQueryHandlerTests
    {
        private readonly Mock<IStockMovementRepository> _stockMovementRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly GetAllStockMovementsViewsQueryHandler _handler;

        private const string _createdByUserId = "123";
        private const string _unknownUserName = "Unknown";
        
        public GetAllStockMovementsViewsQueryHandlerTests()
        {
            _stockMovementRepositoryMock = new Mock<IStockMovementRepository>();
            _userServiceMock = new Mock<IUserService>();
            _handler = new GetAllStockMovementsViewsQueryHandler(_stockMovementRepositoryMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnStockMovementResponses_WhenDataExists()
        {
            // Arrange
            string createdByName = "Worker";
            var movement = TestDataBuilder.CreateStockMovement(createdByUserId: _createdByUserId);
            var movements = new List<StockMovement> { movement };

            _stockMovementRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(movements);

            _userServiceMock
                .Setup(x => x.GetUsersAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(new Dictionary<string, string>
                {
                    [_createdByUserId] = createdByName
                });

            var query = new GetAllStockMovementsViewsQuery();

            // Act
            var result = (await _handler.Handle(query, CancellationToken.None)).ToList();

            // Assert
            result.Should().HaveCount(1);

            var response = result.Single();

            response.Id.Should().Be(movement.Id);
            response.ProductSku.Should().Be(movement.ProductSku);
            response.ProductName.Should().Be(movement.ProductName);
            response.LocationCode.Should().Be(movement.LocationCode);
            response.QuantityChange.Should().Be(movement.QuantityChange);
            response.CreatedByUserId.Should().Be(_createdByUserId);
            response.CreatedByName.Should().Be(createdByName);

            _stockMovementRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _userServiceMock.Verify(x => x.GetUsersAsync(It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldSetCreatedByNameToUnknown_WhenUserDoesNotExist()
        {
            // Arrange
            var movement = TestDataBuilder.CreateStockMovement(
                createdByUserId: _createdByUserId);

            _stockMovementRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { movement });

            _userServiceMock
                .Setup(x => x.GetUsersAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(new Dictionary<string, string>());

            // Act
            var result = await _handler.Handle(new GetAllStockMovementsViewsQuery(), CancellationToken.None);

            // Assert
            result.Single().CreatedByName.Should().Be(_unknownUserName);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoStockMovementsExist()
        {
            // Arrange
            _stockMovementRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<StockMovement>());

            // Act
            var result = await _handler.Handle(new GetAllStockMovementsViewsQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEmpty();

            _stockMovementRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _userServiceMock.Verify(x => x.GetUsersAsync(It.IsAny<IEnumerable<string>>()), Times.Never);
        }
    }
}