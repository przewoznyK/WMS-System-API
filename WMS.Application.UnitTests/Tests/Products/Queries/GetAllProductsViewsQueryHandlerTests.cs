using FluentAssertions;
using Moq;
using WMS.Application.Products.Queries;
using WMS.Application.Products.Response;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;
using WMS.Tests.Common;

namespace WMS.Application.UnitTests.Tests.Products.Queries
{
    public class GetAllProductsViewsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetAllProductsViewsQueryHandler _handler;

        public GetAllProductsViewsQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new GetAllProductsViewsQueryHandler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductViews_WhenProductsExist()
        {
            // Arrange
            string sku1 = "SKU-001";
            string name1 = "Product 1";
            string sku2 = "SKU-002";
            string name2 = "Product 2";

            var products = new List<Product>
            {
                TestDataBuilder.CreateProduct(sku1, name1),
                TestDataBuilder.CreateProduct(sku2, name2)
            };

            _productRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _handler.Handle(new GetAllProductsViewsQuery(),CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(new[]
            {
                new ProductResponse { Sku = sku1, Name = name1 },
                new ProductResponse { Sku = sku2, Name = name2 }
            });

            _productRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            _productRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _handler.Handle(new GetAllProductsViewsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _productRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}