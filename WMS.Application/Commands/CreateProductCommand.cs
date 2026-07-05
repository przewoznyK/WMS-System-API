using MediatR;

namespace WMS.Application.Commands
{
    public record CreateProductCommand(string Sku, string Name, string Description) : IRequest<Guid>;
}
