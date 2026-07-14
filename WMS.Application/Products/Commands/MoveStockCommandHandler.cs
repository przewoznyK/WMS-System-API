using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Commands
{
    internal class MoveStockCommandHandler : IRequestHandler<MoveStockCommand>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MoveStockCommandHandler(IStockRepository stockRepository, IStockMovementRepository stockMovementRepository, IWarehouseLocationRepository warehouseLocationRepository, IUnitOfWork unitOfWork)
        {
            _stockRepository = stockRepository;
            _stockMovementRepository = stockMovementRepository;
            _warehouseLocationRepository = warehouseLocationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MoveStockCommand request, CancellationToken cancellationToken)
        {
            if (request.SourceLocationCode == request.DestinationLocationCode)
            {
                throw new WmsBusinessRuleException("Source and destination location must be different.");
            }

            var sourceStock = await _stockRepository.GetByProductSkuAndLocationCodeAsync(request.ProductSku, request.SourceLocationCode, cancellationToken);

            if (sourceStock == null)
            {
                throw new WmsNotFoundException($"Stock for Product {request.ProductSku} and Location {request.SourceLocationCode} was not found.");
            }

            var destStock = await _stockRepository.GetByProductSkuAndLocationCodeAsync(request.ProductSku, request.DestinationLocationCode, cancellationToken);

            if (destStock == null)
            {
                var destinationLocation = await _warehouseLocationRepository.GetByCodeAsync(request.DestinationLocationCode, cancellationToken);
                if (destinationLocation == null)
                {
                    throw new WmsNotFoundException($"Destination location {request.DestinationLocationCode} does not exist.");
                }

                destStock = new Stock(sourceStock.Product, destinationLocation, 0);
                await _stockRepository.Add(destStock);
            }

            sourceStock.DecreaseQuantity(request.Quantity);
            destStock.IncreaseQuantity(request.Quantity);

            var sourceMovement = new StockMovement(sourceStock, OperationType.Transfer, -request.Quantity);
            var destMovement = new StockMovement(destStock, OperationType.Transfer, request.Quantity);

            await _stockMovementRepository.AddRangeAsync(new[] { sourceMovement, destMovement }, cancellationToken);

            if (sourceStock.Quantity == 0)
            {
                await _stockRepository.Delete(sourceStock);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}