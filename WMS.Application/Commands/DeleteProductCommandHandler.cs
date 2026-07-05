using MediatR;
using WMS.Domain.Exceptions;

namespace WMS.Application.Commands
{
    internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.Id);

            if(product == null)
            {
                throw new ProductNotFoundException("Product not found");
            }

            await _productRepository.DeleteAsync(product);
        }
    }
}