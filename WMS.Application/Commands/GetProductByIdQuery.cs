using MediatR;
using WMS.Domain.Entities;

namespace WMS.Application.Commands
{
    public record GetProductByIdQuery(Guid id) : IRequest<Product>;
}