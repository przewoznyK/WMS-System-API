using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Tests.Common
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

        public static StockMovement CreateStockMovement(
    Stock? stock = null,
    OperationType operationType = OperationType.Receive,
    int quantityChange = 10,
    string createdByUserId = "test-user",
    IssueType? issueType = null,
    string? referenceNumber = null)
        {
            stock ??= CreateStock();

            return new StockMovement(
                stock,
                operationType,
                quantityChange,
                createdByUserId,
                issueType,
                referenceNumber);
        }
    }
}