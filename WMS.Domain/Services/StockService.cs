using WMS.Domain.Entities;

namespace WMS.Domain.Services
{
    public class StockService
    {
        public IEnumerable<StockMovement> MoveProduct(Stock sourceStock, Stock destinationStock, int quantity)
        {
            sourceStock.DecreaseQuantity(quantity);
            destinationStock.IncreaseQuantity(quantity);

            var movements = new List<StockMovement>
            {
                new(sourceStock.ProductId, sourceStock.LocationId, -quantity),
                new(destinationStock.ProductId, destinationStock.LocationId, quantity)
            };

            return movements;
        }
    }
}
