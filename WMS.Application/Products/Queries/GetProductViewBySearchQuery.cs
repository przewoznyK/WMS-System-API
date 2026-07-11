using MediatR;
using WMS.Application.Products.Response;

namespace WMS.Application.Products.Queries
{
    public record GetProductViewBySearchQuery(string searchTerm) : IRequest<ProductResponse>;
}