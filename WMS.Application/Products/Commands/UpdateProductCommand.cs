using MediatR;

namespace WMS.Application.Products.Commands
{
    public record UpdateProductCommand(Guid Id, string Name, string Description) : IRequest;
}