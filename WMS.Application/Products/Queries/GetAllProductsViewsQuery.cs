using MediatR;
using WMS.Application.Products.Response;

namespace WMS.Application.Products.Queries
{
    public record GetAllProductsViewsQuery() : IRequest<IEnumerable<ProductResponse>>;

}
