using MediatR;

namespace WMS.Application.Commands
{
    public record DeleteProductCommand(Guid Id) : IRequest;
}
