using MediatR;
using WMS.Domain;

namespace WMS.Application.Commands
{
    public record GetAllProductsQuery() : IRequest<IEnumerable<Product>>;
}