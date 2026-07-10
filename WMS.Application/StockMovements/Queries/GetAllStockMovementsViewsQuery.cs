using MediatR;
using WMS.Application.StockMovements.Dtos;

namespace WMS.Application.Stocks.Queries
{
    public record GetAllStockMovementsViewsQuery() : IRequest<IEnumerable<StockMovementDto>>;
}
