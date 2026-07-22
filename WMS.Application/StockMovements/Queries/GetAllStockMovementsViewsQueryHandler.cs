using MediatR;
using WMS.Application.Authentication.Interfaces;
using WMS.Application.StockMovements.Response;
using WMS.Domain.Repositories;

namespace WMS.Application.StockMovements.Queries
{
    internal class GetAllStockMovementsViewsQueryHandler : IRequestHandler<GetAllStockMovementsViewsQuery, IEnumerable<StockMovementResponse>>
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IUserService _userService;

        public GetAllStockMovementsViewsQueryHandler(IStockMovementRepository stockMovementRepository, IUserService userService)
        {
            _stockMovementRepository = stockMovementRepository;
            _userService = userService;
        }

        public async Task<IEnumerable<StockMovementResponse>> Handle(GetAllStockMovementsViewsQuery request, CancellationToken cancellationToken)
        {
            var stockMovements = await _stockMovementRepository.GetAllAsync(cancellationToken);

            if (!stockMovements.Any())
                return Enumerable.Empty<StockMovementResponse>();

            var users = await _userService.GetUsersAsync(stockMovements.Select(x => x.CreatedByUserId));

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
                CreatedByUserId = s.CreatedByUserId,
                CreatedByName = users.TryGetValue(s.CreatedByUserId, out var userName) ? userName : "Unknown",
                CreatedAt = s.CreatedAt,
                IssueType = s.IssueType,
                ReferenceNumber = s.ReferenceNumber
            });

            return result.ToList();
        }
    }
}