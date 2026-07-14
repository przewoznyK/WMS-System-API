using MediatR;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Queries
{
    internal class GetProductNamesQueryHandler : IRequestHandler<GetProductNamesQuery, IEnumerable<string>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductNamesQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetProductNamesQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetNamesAsync(cancellationToken);
        }
    }
}