using MediatR;
using WMS.Application.Products.Queries;
using WMS.Application.Stocks.Responses;
using WMS.Domain.Repositories;

namespace WMS.Application.Stocks.Queries
{
    internal class GetLowStockProductsQueryHandler: IRequestHandler<GetLowStockProductsQuery, IEnumerable<LowStockProductResponse>>
    {
        private readonly IStockRepository _stockRepository;

        public GetLowStockProductsQueryHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<IEnumerable<LowStockProductResponse>> Handle(GetLowStockProductsQuery request, CancellationToken cancellationToken)
        {
            var lowStockProducts =await _stockRepository.GetLowStockAsync(request.Threshold, cancellationToken);

            return lowStockProducts.Select(x => new LowStockProductResponse
            {
                ProductSku = x.Sku,
                ProductName = x.Name,
                Quantity = x.Quantity
            });
        }
    }
}