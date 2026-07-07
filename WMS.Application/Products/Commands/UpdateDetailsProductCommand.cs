using MediatR;

namespace WMS.Application.Products.Commands
{
    public record UpdateDetailsProductCommand(Guid Id, string Name, string Description) : IRequest;
}