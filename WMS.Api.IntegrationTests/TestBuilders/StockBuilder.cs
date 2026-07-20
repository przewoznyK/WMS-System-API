using WMS.Domain.Entities;

namespace WMS.Api.IntegrationTests.TestBuilders
{
    public class StockBuilder
    {
        private Product _product;
        private WarehouseLocation _location;
        private int _quantity;

        public StockBuilder()
        {
            _product = new Product($"SKU-001-{Guid.NewGuid()}", "Default Product");
            _location = new WarehouseLocation("XX-00-00");
            _quantity = 10;
        }

        public StockBuilder WithQuantity(int quantity)
        {
            _quantity = quantity;
            return this;
        }

        public StockBuilder WithProduct(string sku, string name = "Test Product")
        {
            _product = new Product(sku, name);
            return this;
        }

        public StockBuilder WithProduct(Product product)
        {
            _product = product;
            return this;
        }

        public StockBuilder WithLocation(string code)
        {
            _location = new WarehouseLocation(code);
            return this;
        }

        public StockBuilder WithLocation(WarehouseLocation location)
        {
            _location = location;
            return this;
        }

        public Stock Build()
        {
            return new Stock(_product, _location, _quantity);
        }
    }
}
