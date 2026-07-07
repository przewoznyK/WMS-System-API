using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Commands
{
    internal class UpdateDetailsProductCommandHandler : IRequestHandler<UpdateDetailsProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public UpdateDetailsProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(UpdateDetailsProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new WmsNotFoundException(nameof(Product), request.Id);
            }

            await _productRepository.UpdateDetailsAsync(product, request.Name, request.Description);
        }
    }
}