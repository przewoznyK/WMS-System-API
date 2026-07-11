using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Commands
{
    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var isSkuTaken = await _productRepository.ExistsBySkuAsync(request.Sku);

            if (isSkuTaken)
            {
                throw new WmsAlreadyExistsException(nameof(Product), nameof(request.Sku), request.Sku);
            }

            Product newProduct = new(request.Sku, request.Name, request.Description);

            await _productRepository.Add(newProduct);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newProduct.Id;
        }
    }
}