using MediatR;
using WMS.Application.Products.Response;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Queries
{
    internal class GetProductBySkuOrNameQueryHandler : IRequestHandler<GetProductBySkuOrNameQuery, ProductResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetProductBySkuOrNameQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> Handle(GetProductBySkuOrNameQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetBySkuOrNameAsync(request.SkuOrName, cancellationToken);

            if (product == null)
            {
                throw new WmsNotFoundException(nameof(Product), request.SkuOrName);
            }

            return new ProductResponse
            {
                Sku = product.Sku,
                Name = product.Name,
                Description = product.Description
            };
        }
    }
}