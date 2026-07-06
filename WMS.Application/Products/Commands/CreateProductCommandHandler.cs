using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Commands
{
    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product newProduct = new(request.Sku, request.Name, request.Description);
            await _productRepository.AddAsync(newProduct);

            return newProduct.Id;
        }
    }
}