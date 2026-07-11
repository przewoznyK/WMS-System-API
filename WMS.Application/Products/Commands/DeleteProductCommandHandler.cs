using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Commands
{
    internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new WmsNotFoundException(nameof(Product), request.Id);
            }

            await _productRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}