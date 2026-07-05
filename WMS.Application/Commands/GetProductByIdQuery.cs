using MediatR;
using WMS.Domain;

namespace WMS.Application.Commands
{
    public record GetProductByIdQuery(Guid id) : IRequest<Product>;
}