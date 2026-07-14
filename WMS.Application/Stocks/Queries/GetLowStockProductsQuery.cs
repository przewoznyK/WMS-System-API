using MediatR;
using WMS.Application.Stocks.Responses;

namespace WMS.Application.Products.Queries
{
    public record GetLowStockProductsQuery(int Threshold) : IRequest<IEnumerable<LowStockProductResponse>>;
}