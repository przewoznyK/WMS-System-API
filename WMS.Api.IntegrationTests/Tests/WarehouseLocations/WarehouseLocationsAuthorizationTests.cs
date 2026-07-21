using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WMS.Api.IntegrationTests.Helpers;
using WMS.Application.Products.Request;
using WMS.Application.WarehouseLocations.Commands;
using WMS.Tests.Common;

namespace WMS.Api.IntegrationTests.Tests.WarehouseLocations
{
    public class WarehouseLocationsAuthorizationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public WarehouseLocationsAuthorizationTests(CustomWebApplicationFactory factory)
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

            var request = new CreateWarehouseLocationCommand(TestConstants.LocationCode, TestConstants.LocationDescription);

            // Act
            var response = await _client.PostAsJsonAsync("/api/warehouselocations/create", request);

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

            var location = TestDataBuilder.CreateWarehouseLocation();
            db.WarehouseLocations.Add(location);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/warehouselocations/delete-{location.Id}");

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

            var location = TestDataBuilder.CreateWarehouseLocation();
            db.WarehouseLocations.Add(location);
            await db.SaveChangesAsync();

            var request = new UpdateDetailsWarehouseLocationRequest
            {
                LocationCode = "AB-02-02",
                Description = "Updated"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/warehouselocations/update-details-{location.Id}", request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}
