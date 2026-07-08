using MediatR;

namespace WMS.Application.Products.Queries
{
    public record GetProductNamesQuery() : IRequest<IEnumerable<string>>;
}