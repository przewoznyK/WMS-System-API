using FluentAssertions;
using Moq;
using WMS.Application.StockMovements.Queries;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.StockMovements.Queries
{ 
    public class GetAllStockMovementsQueryHandlerTests
    {
        private readonly Mock<IStockMovementRepository> _stockMovementRepositoryMock;
        private readonly GetAllStockMovementsQueryHandler _handler;

        public GetAllStockMovementsQueryHandlerTests()
        {
            _stockMovementRepositoryMock = new Mock<IStockMovementRepository>();
            _handler = new GetAllStockMovementsQueryHandler(_stockMovementRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllStockMovements_WhenDataExists()
        {
            // Arrange
            var stockMovements = new List<StockMovement>
            {
                TestDataBuilder.CreateStockMovement(),
                TestDataBuilder.CreateStockMovement(),
                TestDataBuilder.CreateStockMovement()
            };

            _stockMovementRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stockMovements);

            var query = new GetAllStockMovementsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Select(x => x.Id)
                .Should()
                .BeEquivalentTo(stockMovements.Select(x => x.Id));

            _stockMovementRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoStockMovementsExist()
        {
            // Arrange
            _stockMovementRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<StockMovement>());

            var query = new GetAllStockMovementsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _stockMovementRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}