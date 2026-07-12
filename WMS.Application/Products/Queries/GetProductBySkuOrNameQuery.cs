using MediatR;
using WMS.Application.Products.Response;

namespace WMS.Application.Products.Queries
{
    public record GetProductBySkuOrNameQuery(string SkuOrName) : IRequest<ProductResponse>;
}