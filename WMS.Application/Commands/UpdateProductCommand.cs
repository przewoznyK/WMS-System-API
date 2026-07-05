using MediatR;
using WMS.Domain;

namespace WMS.Application.Commands
{
    public record UpdateProductCommand(Guid Id, string Name, string Description) : IRequest<Product>;
}