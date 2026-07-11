using MediatR;
using WMS.Application.Stocks.Dtos;

namespace WMS.Application.Stocks.Queries
{
    public record GetAllStocksViewsByProductSkuQuery(string sku) : IRequest<IEnumerable<StockDto>>;
}
