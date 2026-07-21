using MediatR;

namespace WMS.Application.Products.Commands
{
    public record DeleteProductCommand(Guid Id) : IRequest;
}