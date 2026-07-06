using MediatR;
using WMS.Domain.Entities;

namespace WMS.Application.Commands
{
    public record GetAllProductsQuery() : IRequest<IEnumerable<Product>>;
}