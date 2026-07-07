using MediatR;
using WMS.Application.Stocks.Dtos;

namespace WMS.Application.Stocks.Queries
{
    public record GetAllStocksViewsQuery() : IRequest<IEnumerable<StockDto>>;

}
