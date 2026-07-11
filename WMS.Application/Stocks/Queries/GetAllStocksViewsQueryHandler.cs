using MediatR;
using WMS.Application.Stocks.Response;
using WMS.Domain.Repositories;

namespace WMS.Application.Stocks.Queries
{
    internal class GetAllStocksViewsQueryHandler : IRequestHandler<GetAllStocksViewsQuery, IEnumerable<StockResponse>>
    {
        private readonly IStockRepository _stockRepository;

        public GetAllStocksViewsQueryHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<IEnumerable<StockResponse>> Handle(GetAllStocksViewsQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _stockRepository.GetAllAsync();

            return stocks.Select(s => new StockResponse
            {
                ProductSku = s.Product.Sku,
                ProductName = s.Product.Name,
                LocationCode = s.Location.Code,
                Quantity = s.Quantity
            });
        }
    }
}