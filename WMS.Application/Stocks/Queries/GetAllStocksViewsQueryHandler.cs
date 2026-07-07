using MediatR;
using WMS.Application.Stocks.Dtos;
using WMS.Domain.Repositories;

namespace WMS.Application.Stocks.Queries
{
    internal class GetAllStocksViewsQueryHandler : IRequestHandler<GetAllStocksViewsQuery, IEnumerable<StockDto>>
    {
        private readonly IStockRepository _stockRepository;

        public GetAllStocksViewsQueryHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<IEnumerable<StockDto>> Handle(GetAllStocksViewsQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _stockRepository.GetAllAsync();

            return stocks.Select(s => new StockDto
            {
                ProductName = s.Product?.Name ?? "Unknown",
                LocationCode = s.Location?.Code ?? "Null",
                Quantity = s.Quantity
            });
        }
    }
}