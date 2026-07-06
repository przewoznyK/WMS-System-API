using MediatR;
using WMS.Domain.Entities;

namespace WMS.Application.Products.Queries
{
    public record GetProductByIdQuery(Guid Id) : IRequest<Product>;
}