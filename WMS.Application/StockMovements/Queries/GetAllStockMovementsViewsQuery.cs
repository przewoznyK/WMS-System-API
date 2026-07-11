using MediatR;
using WMS.Application.StockMovements.Response;

namespace WMS.Application.Stocks.Queries
{
    public record GetAllStockMovementsViewsQuery() : IRequest<IEnumerable<StockMovementResponse>>;
}