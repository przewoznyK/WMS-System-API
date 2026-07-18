using WMS.Domain.Entities;

namespace WMS.Api.IntegrationTests
{
    public static class TestDataBuilder
    {
        public static Product CreateProduct(string sku = "SKU-001-", string name = "Default Product")
        {
            return new Product(sku, name);
        }

        public static WarehouseLocation CreateLocation(string code = "LOC-01")
        {
            return new WarehouseLocation(code);
        }

        public static Stock CreateStock(Product? product = null, WarehouseLocation? location = null, int quantity = 10)
        {
            return new Stock(
                product ?? CreateProduct(),
                location ?? CreateLocation(),
                quantity
            );
        }
    }
}
