using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using FluentAssertions;
using WMS.Api.IntegrationTests.Helpers;
using WMS.Application.Products.Commands;
using WMS.Application.Products.Request;
using WMS.Tests.Common;

namespace WMS.Api.IntegrationTests.Tests.Products
{
    public class ProductsAuthorizationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public ProductsAuthorizationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "FakeToken");
        }

        [Theory]
        [InlineData("Manager", HttpStatusCode.OK)]
        [InlineData("Worker", HttpStatusCode.Forbidden)]
        public async Task Create_ShouldReturnExpectedStatusCode_ForRole(string role, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            TestAuthHandler.Role = role;

            using var database = _factory.CreateDatabase();

            var request = new CreateProductCommand(TestConstants.ProductSku, TestConstants.ProductName, null);

            // Act
            var response = await _client.PostAsJsonAsync("/api/products/create", request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Theory]
        [InlineData("Manager", HttpStatusCode.NoContent)]
        [InlineData("Worker", HttpStatusCode.Forbidden)]
        public async Task Delete_ShouldReturnExpectedStatusCode_ForRole(string role, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            TestAuthHandler.Role = role;

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var product = TestDataBuilder.CreateProduct();
            db.Products.Add(product);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/products/delete-{product.Id}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Theory]
        [InlineData("Manager", HttpStatusCode.NoContent)]
        [InlineData("Worker", HttpStatusCode.Forbidden)]
        public async Task Update_ShouldReturnExpectedStatusCode_ForRole(string role, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            TestAuthHandler.Role = role;

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var product = TestDataBuilder.CreateProduct();
            db.Products.Add(product);
            await db.SaveChangesAsync();

            var request = new UpdateDetailsProductRequest
            {
                Name = "Updated product",
                Description = "Updated"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/update-details-{product.Id}", request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}