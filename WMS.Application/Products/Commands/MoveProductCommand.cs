using MediatR;

namespace WMS.Application.Products.Commands
{
    public record MoveProductCommand(Guid ProductId, Guid SourceLocationId, Guid DestinationLocationId, int Quantity) : IRequest;
}