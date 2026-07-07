using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Application.Stocks.Queries
{
    internal class GetAllStocksQueryHandler : IRequestHandler<GetAllStocksQuery, IEnumerable<Stock>>
    {
        private readonly IStockRepository _stockRepository;

        public GetAllStocksQueryHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<IEnumerable<Stock>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
        {
            return await _stockRepository.GetAllAsync();
        }
    }
}