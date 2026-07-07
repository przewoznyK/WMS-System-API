using MediatR;
using WMS.Domain.Entities;

namespace WMS.Application.Stocks.Queries
{
    public record GetAllStocksQuery() : IRequest<IEnumerable<Stock>>;

}
