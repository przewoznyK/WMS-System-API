using MediatR;

namespace WMS.Application.Commands.Products.Queries
{
    public record CreateProductCommand(string Sku, string Name, string Description) : IRequest<Guid>;
}
