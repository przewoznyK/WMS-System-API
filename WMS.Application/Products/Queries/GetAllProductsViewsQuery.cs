using MediatR;
using WMS.Application.Products.Dtos;

namespace WMS.Application.Products.Queries
{
    public record GetAllProductsViewsQuery() : IRequest<IEnumerable<ProductDto>>;

}
