using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WMS.Api.IntegrationTests.Helpers;
using WMS.Application.Products.Request;
using WMS.Application.WarehouseLocations.Commands;
using WMS.Tests.Common;

namespace WMS.Api.IntegrationTests.Tests.WarehouseLocations
{
    public class WarehouseLocationsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public WarehouseLocationsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "FakeToken");
        }

        [Fact]
        public async Task GetCodesAsync_ShouldReturnLocations_WhenDataExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var location = TestDataBuilder.CreateWarehouseLocation();
            db.WarehouseLocations.Add(location);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/warehouselocations/location-codes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<string>>();
            result.Should().NotBeNull();
            result.Should().ContainSingle();
            result.Should().Contain(TestConstants.LocationCode);
        }

        [Fact]
        public async Task GetCodesAsync_ShouldReturnEmptyList_WhenDatabaseIsEmpty()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            // Act
            var response = await _client.GetAsync("/api/warehouselocations/location-codes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<string>>();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Create_ShouldCreateLocation_WhenDataIsValid()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var request = new CreateWarehouseLocationCommand(TestConstants.LocationCode, TestConstants.LocationDescription);

            // Act
            var response = await _client.PostAsJsonAsync("/api/warehouselocations/create", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var locationId = await response.Content.ReadFromJsonAsync<Guid>();
            locationId.Should().NotBeEmpty();

            var location = await db.WarehouseLocations.AsNoTracking().FirstAsync(x => x.Id == locationId);
            location.Code.Should().Be(TestConstants.LocationCode);
        }

        [Fact]
        public async Task Create_ShouldReturnConflict_WhenCodeAlreadyExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var location = TestDataBuilder.CreateWarehouseLocation();
            db.WarehouseLocations.Add(location);
            await db.SaveChangesAsync();

            var request = new CreateWarehouseLocationCommand(TestConstants.LocationCode, TestConstants.LocationDescription);

            // Act
            var response = await _client.PostAsJsonAsync("/api/warehouselocations/create", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Theory]
        [InlineData("INVALID")]
        [InlineData("ABC-123")]
        [InlineData("AA-1-22")]
        [InlineData("12-AB-34")]
        [InlineData("A1-22-33")]
        public async Task Create_ShouldReturnBadRequest_WhenLocationCodeFormatIsInvalid(string code)
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            var request = new CreateWarehouseLocationCommand(code, "Test location");

            // Act
            var response = await _client.PostAsJsonAsync("/api/warehouselocations/create", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("The format must comply with the pattern: AA-00-00.");
        }

        [Fact]
        public async Task Update_ShouldUpdateLocation_WhenDataIsValid()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var location = TestDataBuilder.CreateWarehouseLocation();
            db.WarehouseLocations.Add(location);
            await db.SaveChangesAsync();

            string updatedLocationCode = "BB-01-01";
            string updatedDescription = "Updated description";

            var request = new UpdateDetailsWarehouseLocationRequest
            {
                LocationCode = updatedLocationCode,
                Description = updatedDescription
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/warehouselocations/update-details-{location.Id}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var updated = await db.WarehouseLocations
                .AsNoTracking()
                .FirstAsync(x => x.Id == location.Id);

            updated.Code.Should().Be(updatedLocationCode);
            updated.Description.Should().Be(updatedDescription);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenLocationDoesNotExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var id = Guid.NewGuid();

            var request = new UpdateDetailsWarehouseLocationRequest
            {
                LocationCode = TestConstants.LocationCode,
                Description = TestConstants.LocationDescription
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/warehouselocations/update-details-{id}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("INVALID")]
        [InlineData("ABC-123")]
        [InlineData("AA-1-22")]
        [InlineData("12-AB-34")]
        [InlineData("A1-22-33")]
        public async Task Update_ShouldReturnBadRequest_WhenLocationCodeFormatIsInvalid(string invalidCode)
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var location = TestDataBuilder.CreateWarehouseLocation();

            db.WarehouseLocations.Add(location);
            await db.SaveChangesAsync();

            var request = new UpdateDetailsWarehouseLocationRequest
            {
                LocationCode = invalidCode,
                Description = "Updated description"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/warehouselocations/update-details-{location.Id}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("The format must comply with the pattern: AA-00-00.");
        }

        [Fact]
        public async Task Delete_ShouldDeleteLocation_WhenLocationExists()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();
            var db = database.Db;

            var location = TestDataBuilder.CreateWarehouseLocation();
            db.WarehouseLocations.Add(location);
            await db.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/warehouselocations/delete-{location.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var deleted = await db.WarehouseLocations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == location.Id);

            deleted.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenLocationDoesNotExist()
        {
            // Arrange
            TestAuthHandler.Role = "Manager";

            using var database = _factory.CreateDatabase();

            // Act
            var response = await _client.DeleteAsync($"/api/warehouselocations/delete-{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}