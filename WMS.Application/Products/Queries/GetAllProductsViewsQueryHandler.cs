using MediatR;
using WMS.Application.Products.Response;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Queries
{
    internal class GetAllProductsViewsQueryHandler : IRequestHandler<GetAllProductsViewsQuery, IEnumerable<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsViewsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductsViewsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);

            return products.Select(s => new ProductResponse
            {
                Sku = s.Sku,
                Name = s.Name,
                Description = s.Description
            });
        }
    }
}