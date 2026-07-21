using FluentAssertions;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using Xunit;

namespace WMS.Domain.UnitTests.Tests
{
    public class ProductTests
    {
        #region Constructor

        [Fact]
        public void Constructor_ShouldCreateProduct_WhenDataIsValid()
        {
            // Arrange
            var sku = "SKU";
            var name = "Name";
            var description = "Description";

            // Act
            var product = new Product(sku, name, description);

            // Assert
            product.Id.Should().NotBeEmpty();
            product.Sku.Should().Be(sku);
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowException_WhenSkuIsEmpty(string? sku)
        {
            // Arrange
            var name = "Name";

            // Act
            Action action = () => new Product(sku!, name);

            // Assert
            action.Should().Throw<WmsNullOrEmptyException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowException_WhenNameIsEmpty(string? name)
        {
            // Act
            Action action = () => new Product("SKU", name!, "Description");

            // Assert
            action.Should().Throw<WmsNullOrEmptyException>();
        }

        [Fact]
        public void Constructor_ShouldSetEmptyDescription_WhenDescriptionIsNotProvided()
        {
            // Act
            var product = new Product("SKU", "Name");

            // Assert
            product.Description.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_ShouldSetEmptyDescription_WhenDescriptionIsNull()
        {
            // Act
            var product = new Product("SKU", "Name", null);

            // Assert
            product.Description.Should().BeEmpty();
        }

        #endregion

        #region UpdateDetails

        [Fact]
        public void UpdateDetails_ShouldUpdateProduct_WhenDataIsValid()
        {
            // Arrange
            var sku = "SKU";
            var product = new Product(sku, "Name");
            var productId = product.Id;
            var updatedName = "Updated Name";
            var updatedDescription = "Updated Description";

            // Act
            product.UpdateDetails(updatedName, updatedDescription);

            // Assert
            product.Id.Should().Be(productId);
            product.Sku.Should().Be(sku);
            product.Name.Should().Be(updatedName);
            product.Description.Should().Be(updatedDescription);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UpdateDetails_ShouldThrowException_WhenNameIsEmpty(string? name)
        {
            // Arrange
            var product = new Product("SKU", "Name");
            var updatedDescription = "Updated Description";

            // Act
            Action action = () => product.UpdateDetails(name!, updatedDescription);

            // Assert
            action.Should().Throw<WmsNullOrEmptyException>();
        }

        [Fact]
        public void UpdateDetails_ShouldSetEmptyDescription_WhenDescriptionIsNull()
        {
            // Arrange
            var product = new Product("SKU", "Name");

            // Act
            product.UpdateDetails("Updated Name", null);

            // Assert
            product.Description.Should().BeEmpty();
        }

        #endregion
    }
}