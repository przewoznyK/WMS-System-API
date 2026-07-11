using MediatR;
using WMS.Application.StockMovements.Dtos;
using WMS.Domain.Repositories;

namespace WMS.Application.Stocks.Queries
{
    internal class GetAllStockMovementsViewsQueryHandler : IRequestHandler<GetAllStockMovementsViewsQuery, IEnumerable<StockMovementDto>>
    {
        private readonly IStockMovementRepository _stockMovementRepository;

        public GetAllStockMovementsViewsQueryHandler(IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<IEnumerable<StockMovementDto>> Handle(GetAllStockMovementsViewsQuery request, CancellationToken cancellationToken)
        {
            var stockMovements = await _stockMovementRepository.GetAllAsync();

            var result = stockMovements.Select(s => new StockMovementDto
            {
                Id = s.Id,
                ProductId = s.ProductId,
                LocationId = s.LocationId,
                ProductSku = s.ProductSku,
                ProductName = s.ProductName,
                LocationCode = s.LocationCode,
                QuantityChange = s.QuantityChange,
                CreatedAt = s.CreatedAt
            });

            return result.ToList();
        }
    }
}