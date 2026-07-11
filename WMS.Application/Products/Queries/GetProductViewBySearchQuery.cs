using MediatR;
using WMS.Application.Products.Dtos;

namespace WMS.Application.Products.Queries
{
    public record GetProductViewBySearchQuery(string searchTerm) : IRequest<ProductDto>;
}