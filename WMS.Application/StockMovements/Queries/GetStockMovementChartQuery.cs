using MediatR;
using WMS.Application.StockMovements.Responses;

namespace WMS.Application.StockMovements.Queries
{
    public record GetStockMovementChartQuery(int Days) : IRequest<IEnumerable<StockMovementChartResponse>>;
}
