using WMS.Domain.Entities;
using WMS.Domain.Enums;

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
                new(sourceStock, OperationType.Transfer, -quantity),
                new(destinationStock, OperationType.Transfer, quantity)
            };

            return movements;
        }
    }
}
