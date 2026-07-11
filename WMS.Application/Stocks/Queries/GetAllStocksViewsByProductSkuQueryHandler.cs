using MediatR;
using WMS.Application.Stocks.Dtos;
using WMS.Domain.Repositories;

namespace WMS.Application.Stocks.Queries
{
    internal class GetAllStocksViewsByProductSkuQueryHandler : IRequestHandler<GetAllStocksViewsByProductSkuQuery, IEnumerable<StockDto>>
    {
        private readonly IStockRepository _stockRepository;

        public GetAllStocksViewsByProductSkuQueryHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<IEnumerable<StockDto>> Handle(GetAllStocksViewsByProductSkuQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _stockRepository.GetAllByProductSkuAsync(request.sku);

            return stocks.Select(s => new StockDto
            {
                ProductSku = s.Product.Sku,
                ProductName = s.Product.Name,
                LocationCode = s.Location.Code,
                Quantity = s.Quantity
            });
        }
    }
}