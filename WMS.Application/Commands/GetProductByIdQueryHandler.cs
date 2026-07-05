using MediatR;
using WMS.Domain;
using WMS.Domain.Exceptions;

namespace WMS.Application.Commands
{
    internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.id);

            if (product == null)
            {
                throw new ProductNotFoundException("Product not found");
            }

            return product;
        }
    }
}