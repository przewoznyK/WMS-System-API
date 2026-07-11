using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Commands
{
    internal class UpdateDetailsProductCommandHandler : IRequestHandler<UpdateDetailsProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDetailsProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateDetailsProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new WmsNotFoundException(nameof(Product), request.Id);
            }

            product.UpdateDetails(request.Name, request.Description);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}