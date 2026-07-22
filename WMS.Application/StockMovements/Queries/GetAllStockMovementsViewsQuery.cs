using MediatR;
using WMS.Application.StockMovements.Response;

namespace WMS.Application.StockMovements.Queries
{
    public record GetAllStockMovementsViewsQuery() : IRequest<IEnumerable<StockMovementResponse>>;
}