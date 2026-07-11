using MediatR;
using WMS.Application.Stocks.Response;

namespace WMS.Application.Stocks.Queries
{
    public record GetAllStocksViewsByProductSkuQuery(string sku) : IRequest<IEnumerable<StockResponse>>;
}
