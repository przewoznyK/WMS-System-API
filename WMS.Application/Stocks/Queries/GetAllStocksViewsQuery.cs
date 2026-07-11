using MediatR;
using WMS.Application.Stocks.Response;

namespace WMS.Application.Stocks.Queries
{
    public record GetAllStocksViewsQuery() : IRequest<IEnumerable<StockResponse>>;
}
