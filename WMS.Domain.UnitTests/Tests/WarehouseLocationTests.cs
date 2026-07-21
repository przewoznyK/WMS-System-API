
using FluentAssertions;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using Xunit;

namespace WMS.Domain.UnitTests.Tests
{
    public class WarehouseLocationTests
    {
        #region Constructor

        [Fact]
        public void Constructor_ShouldCreateWarehouseLocation_WhenDataIsValid()
        {
            // Arrange
            var code = "AA-01-01";
            var description = "Location Description";

            // Act
            var location = new WarehouseLocation(code, description);

            // Assert
            location.Id.Should().NotBeEmpty();
            location.Code.Should().Be(code);
            location.Description.Should().Be(description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowException_WhenCodeIsEmpty(string? code)
        {
            // Act
            Action action = () => new WarehouseLocation(code!);

            // Assert
            action.Should().Throw<WmsNullOrEmptyException>();
        }

        [Theory]
        [InlineData("A1-01-01")]
        [InlineData("AA0101")]
        [InlineData("AA-1-01")]
        [InlineData("AA-001-01")]
        [InlineData("aa-01-01")]
        [InlineData("AAA-01-01")]
        [InlineData("AA-01")]
        [InlineData("11-11-11")]
        public void Constructor_ShouldThrowException_WhenCodeHasInvalidFormat(string code)
        {
            // Act
            Action action = () => new WarehouseLocation(code);

            // Assert
            action.Should().Throw<WmsBusinessRuleException>();
        }

        [Fact]
        public void Constructor_ShouldSetEmptyDescription_WhenDescriptionIsNotProvided()
        {
            // Arrange
            var location = new WarehouseLocation("AA-01-01");

            // Assert
            location.Description.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_ShouldSetEmptyDescription_WhenDescriptionIsNull()
        {
            // Arrange
            var location = new WarehouseLocation("AA-01-01", null);

            // Assert
            location.Description.Should().BeEmpty();
        }

        #endregion

        #region UpdateDetails

        [Fact]
        public void UpdateDetails_ShouldUpdateWarehouseLocation_WhenDataIsValid()
        {
            // Arrange
            var location = new WarehouseLocation("AA-01-01", "Old Description");
            var id = location.Id;

            var updatedCode = "BB-02-03";
            var updatedDescription = "Updated Description";

            // Act
            location.UpdateDetails(updatedCode, updatedDescription);

            // Assert
            location.Id.Should().Be(id);
            location.Code.Should().Be(updatedCode);
            location.Description.Should().Be(updatedDescription);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UpdateDetails_ShouldThrowException_WhenCodeIsEmpty(string? code)
        {
            // Arrange
            var location = new WarehouseLocation("AA-01-01");

            // Act
            Action action = () => location.UpdateDetails(code!, "Description");

            // Assert
            action.Should().Throw<WmsNullOrEmptyException>();
        }

        [Theory]
        [InlineData("ABC")]
        [InlineData("AA0101")]
        [InlineData("11-11-11")]
        [InlineData("AA-1-01")]
        [InlineData("AA-001-01")]
        [InlineData("aa-01-01")]
        public void UpdateDetails_ShouldThrowException_WhenCodeHasInvalidFormat(string code)
        {
            // Arrange
            var location = new WarehouseLocation("AA-01-01");

            // Act
            Action action = () => location.UpdateDetails(code, "Description");

            // Assert
            action.Should().Throw<WmsBusinessRuleException>();
        }

        [Fact]
        public void UpdateDetails_ShouldSetEmptyDescription_WhenDescriptionIsNull()
        {
            // Arrange
            var location = new WarehouseLocation("AA-01-01");

            // Act
            location.UpdateDetails("BB-02-03", null);

            // Assert
            location.Description.Should().BeEmpty();
        }

        #endregion
    }
}