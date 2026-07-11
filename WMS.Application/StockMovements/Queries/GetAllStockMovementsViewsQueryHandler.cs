using MediatR;
using WMS.Application.StockMovements.Response;
using WMS.Domain.Repositories;

namespace WMS.Application.Stocks.Queries
{
    internal class GetAllStockMovementsViewsQueryHandler : IRequestHandler<GetAllStockMovementsViewsQuery, IEnumerable<StockMovementResponse>>
    {
        private readonly IStockMovementRepository _stockMovementRepository;

        public GetAllStockMovementsViewsQueryHandler(IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<IEnumerable<StockMovementResponse>> Handle(GetAllStockMovementsViewsQuery request, CancellationToken cancellationToken)
        {
            var stockMovements = await _stockMovementRepository.GetAllAsync();

            var result = stockMovements.Select(s => new StockMovementResponse
            {
                Id = s.Id,
                ProductId = s.ProductId,
                LocationId = s.LocationId,
                ProductSku = s.ProductSku,
                ProductName = s.ProductName,
                LocationCode = s.LocationCode,
                OperationType = s.OperationType,
                QuantityChange = s.QuantityChange,
                CreatedAt = s.CreatedAt
            });

            return result.ToList();
        }
    }
}