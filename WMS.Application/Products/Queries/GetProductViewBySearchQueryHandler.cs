using MediatR;
using WMS.Application.Products.Dtos;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Queries
{
    internal class GetProductViewBySearchQueryHandler : IRequestHandler<GetProductViewBySearchQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public GetProductViewBySearchQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> Handle(GetProductViewBySearchQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetBySearch(request.searchTerm);

            if (product == null)
            {
                throw new WmsNotFoundException(nameof(Product), request.searchTerm);
            }

            return new ProductDto
            {
                Sku = product.Sku,
                Name = product.Name,
                Description = product.Description
            };
        }
    }
}