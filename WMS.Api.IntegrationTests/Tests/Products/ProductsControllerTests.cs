using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WMS.Api.IntegrationTests.Helpers;
using WMS.Application.Products.Commands;
using WMS.Application.Products.Request;
using WMS.Domain.Entities;
using WMS.Tests.Common;

namespace WMS.Api.IntegrationTests.Tests.Products
{
    public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public ProductsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "FakeToken");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProducts_WhenDataExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var product = TestDataBuilder.CreateProduct();
            db.Products.Add(product);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/products/all");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
            result.Should().NotBeNull();
            result.Should().ContainSingle();

            var returnedProduct = result!.Single();
            returnedProduct.Sku.Should().Be(TestConstants.ProductSku);
            returnedProduct.Name.Should().Be(TestConstants.ProductName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenDatabaseIsEmpty()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            // Act
            var response = await _client.GetAsync("/api/products/all");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Create_ShouldCreateProduct_WhenDataIsValid()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var request = new CreateProductCommand(TestConstants.ProductSku, TestConstants.ProductName, TestConstants.ProductDescription);

            // Act
            var response = await _client.PostAsJsonAsync("/api/products/create", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var productId = await response.Content.ReadFromJsonAsync<Guid>();
            productId.Should().NotBeEmpty();

            var createdProduct = await db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == productId);

            createdProduct.Should().NotBeNull();
            createdProduct!.Sku.Should().Be(TestConstants.ProductSku);
            createdProduct.Name.Should().Be(TestConstants.ProductName);
        }

        [Fact]
        public async Task Create_ShouldCreateProduct_WhenDescriptionIsNull()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var request = new CreateProductCommand(TestConstants.ProductSku, TestConstants.ProductName, null);

            // Act
            var response = await _client.PostAsJsonAsync("/api/products/create", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var productId = await response.Content.ReadFromJsonAsync<Guid>();
            productId.Should().NotBeEmpty();

            var createdProduct = await db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == productId);

            createdProduct.Should().NotBeNull();
            createdProduct!.Sku.Should().Be(TestConstants.ProductSku);
            createdProduct.Name.Should().Be(TestConstants.ProductName);
            createdProduct.Description.Should().BeEmpty();
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenSkuAlreadyExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var existingProduct = TestDataBuilder.CreateProduct();
            db.Products.Add(existingProduct);
            await db.SaveChangesAsync();

            var request = new CreateProductCommand(
                TestConstants.ProductSku,
                "Another Product",
                null);

            // Act
            var response = await _client.PostAsJsonAsync("/api/products/create", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain(nameof(Product));
            body.Should().Contain(TestConstants.ProductSku);
        }

        [Fact]
        public async Task Update_ShouldUpdateProduct_WhenDataIsValid()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var product = TestDataBuilder.CreateProduct();
            db.Products.Add(product);
            await db.SaveChangesAsync();

            var request = new UpdateDetailsProductRequest
            {
                Name = "Updated Name",
                Description = "Updated Description"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/update-details-{product.Id}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var updatedProduct = await db.Products
                .AsNoTracking()
                .FirstAsync(x => x.Id == product.Id);

            updatedProduct.Name.Should().Be("Updated Name");
            updatedProduct.Description.Should().Be("Updated Description");
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            var id = Guid.NewGuid();
            var request = new UpdateDetailsProductCommand(id, "Updated Name", "Updated Description");

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/update-details-{id}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var product = TestDataBuilder.CreateProduct();
            db.Products.Add(product);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync(
              $"/api/products/delete-{product.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var deletedProduct = await db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == product.Id);

            deletedProduct.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            // Act
            var response = await _client.DeleteAsync($"/api/products/delete-{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
