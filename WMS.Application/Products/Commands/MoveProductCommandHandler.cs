using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Domain.Services;
using WMS.Infrastructure;

namespace WMS.Application.Products.Commands
{
    internal class MoveProductCommandHandler : IRequestHandler<MoveProductCommand>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly StockService _stockService;
        private readonly WmsDbContext _wmsDbContext;

        public MoveProductCommandHandler(IStockRepository stockRepository, IStockMovementRepository stockMovementRepository, StockService stockService, WmsDbContext wmsDbContext)
        {
            _stockRepository = stockRepository;
            _stockMovementRepository = stockMovementRepository;
            _stockService = stockService;
            _wmsDbContext = wmsDbContext;
        }

        public async Task Handle(MoveProductCommand request, CancellationToken cancellationToken)
        {
            if(request.SourceLocationId == request.DestinationLocationId)
            {
                throw new WmsBusinessRuleException("Source and destination location must be different.");
            }

            var sourceStock = await _stockRepository.GetByProductAndLocationAsync(request.ProductId, request.SourceLocationId);

            if (sourceStock == null)
            {
                throw new WmsNotFoundException($"Stock for Product {request.ProductId} and Location {request.SourceLocationId} was not found.");
            }

            var destStock = await _stockRepository.GetByProductAndLocationAsync(request.ProductId, request.DestinationLocationId);
            
            if (destStock == null)
            {
                destStock = new Stock(request.ProductId, request.DestinationLocationId, 0);
                await _stockRepository.AddAsync(destStock);
            }

            var movements = _stockService.MoveProduct(sourceStock, destStock, request.Quantity);

            await _stockMovementRepository.AddRangeAsync(movements);

            await _wmsDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}