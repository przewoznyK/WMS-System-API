using MediatR;

namespace WMS.Application.Products.Commands
{
    public record CreateProductCommand(string Sku, string Name, string? Description) : IRequest<Guid>;
}