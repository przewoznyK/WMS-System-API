using MediatR;
using WMS.Domain.Entities;

namespace WMS.Application.Products.Queries
{
    public record GetAllProductsQuery() : IRequest<IEnumerable<Product>>;
}