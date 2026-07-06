using MediatR;

namespace WMS.Application.Commands.Products.Queries
{
    public record DeleteProductCommand(Guid Id) : IRequest;
}
