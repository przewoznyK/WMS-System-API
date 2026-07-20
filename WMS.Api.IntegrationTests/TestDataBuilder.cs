using WMS.Domain.Entities;

namespace WMS.Api.IntegrationTests
{
    public static class TestDataBuilder
    {
        public static Product CreateProduct(string sku = TestConstants.ProductSku,string name = TestConstants.ProductName)
        {
            return new Product(sku, name);
        }
        public static WarehouseLocation CreateWarehouseLocation(string code = TestConstants.LocationCode)
        {
            return new WarehouseLocation(code);
        }

        public static Stock CreateStock(Product? product = null, WarehouseLocation? location = null, int quantity = 10)
        {
            return new Stock(
                product ?? CreateProduct(),
                location ?? CreateWarehouseLocation(),
                quantity
            );
        }
    }
}