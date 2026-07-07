using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Application.StockMovements.Queries
{
    internal class GetAllStockMovementsQueryHandler : IRequestHandler<GetAllStockMovementsQuery, IEnumerable<StockMovement>>
    {
        private readonly IStockMovementRepository _stockMovementRepository;

        public GetAllStockMovementsQueryHandler(IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<IEnumerable<StockMovement>> Handle(GetAllStockMovementsQuery request, CancellationToken cancellationToken)
        {
            return await _stockMovementRepository.GetAllAsync();
        }
    }
}