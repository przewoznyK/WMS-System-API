using MediatR;
using WMS.Application.Products.Dtos;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Queries
{
    internal class GetAllProductsViewsQueryHandler : IRequestHandler<GetAllProductsViewsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsViewsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsViewsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(s => new ProductDto
            {
                Sku = s.Sku,
                Name = s.Name,
                Description = s.Description
            });
        }
    }
}