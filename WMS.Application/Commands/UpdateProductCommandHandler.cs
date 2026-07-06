using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;

namespace WMS.Application.Commands
{
    internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.Id);

            if (product == null)
            {
                throw new ProductNotFoundException("Product not found");
            }

            await _productRepository.UpdateDetailsAsync(product, request.Name, request.Description);
            return product;
        }
    }
}