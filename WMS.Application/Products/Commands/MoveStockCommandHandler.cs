using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Domain.Services;
using WMS.Infrastructure;

namespace WMS.Application.Products.Commands
{
    internal class MoveStockCommandHandler : IRequestHandler<MoveStockCommand>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;
        private readonly StockService _stockService;
        private readonly WmsDbContext _wmsDbContext;

        public MoveStockCommandHandler(IStockRepository stockRepository, IStockMovementRepository stockMovementRepository, IWarehouseLocationRepository warehouseLocationRepository, StockService stockService, WmsDbContext wmsDbContext)
        {
            _stockRepository = stockRepository;
            _stockMovementRepository = stockMovementRepository;
            _warehouseLocationRepository = warehouseLocationRepository;
            _stockService = stockService;
            _wmsDbContext = wmsDbContext;
        }

        public async Task Handle(MoveStockCommand request, CancellationToken cancellationToken)
        {
            if (request.SourceLocationCode == request.DestinationLocationCode)
            {
                throw new WmsBusinessRuleException("Source and destination location must be different.");
            }

            var sourceStock = await _stockRepository.GetByProductSkuAndLocationCodeAsync(request.ProductSku, request.SourceLocationCode);

            if (sourceStock == null)
            {
                throw new WmsNotFoundException($"Stock for Product {request.ProductSku} and Location {request.SourceLocationCode} was not found.");
            }

            var destStock = await _stockRepository.GetByProductSkuAndLocationCodeAsync(request.ProductSku, request.DestinationLocationCode);

            if (destStock == null)
            {
                var destinationLocation = await _warehouseLocationRepository.GetByCodeAsync(request.DestinationLocationCode);
                if (destinationLocation == null)
                {
                    throw new WmsNotFoundException($"Destination location {request.DestinationLocationCode} does not exist.");
                }

                destStock = new Stock(sourceStock.Product, destinationLocation, 0);
                await _stockRepository.AddAsync(destStock);
            }

            string productName = sourceStock.Product.Name;
            var movements = _stockService.MoveProduct(sourceStock, destStock, request.Quantity);
            
            if (sourceStock.Quantity == 0)
            {
                await _stockRepository.DeleteAsync(sourceStock);
            }

            await _stockMovementRepository.AddRangeAsync(movements);

            await _wmsDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}