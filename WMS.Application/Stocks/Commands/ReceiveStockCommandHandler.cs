using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.Stocks.Commands
{
    internal class ReceiveStockCommandHandler : IRequestHandler<ReceiveStockCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReceiveStockCommandHandler(IProductRepository productRepository, IWarehouseLocationRepository warehouseLocationRepository, IStockRepository stockRepository,
            IStockMovementRepository stockMovementRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _warehouseLocationRepository = warehouseLocationRepository;
            _stockRepository = stockRepository;
            _stockMovementRepository = stockMovementRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(ReceiveStockCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetBySkuAsync(command.ProductSku);

            if (product == null)
            {
                throw new WmsNotFoundException("Product", command.ProductSku);
            }

            var location = await _warehouseLocationRepository.GetByCodeAsync(command.LocationCode);

            if (location == null)
            {
                throw new WmsNotFoundException("WarehouseLocation", command.LocationCode);
            }

            var stock = await _stockRepository.GetByProductIdAndLocationAsync(product.Id, location.Id);

            if (stock != null)
            {
                stock.IncreaseQuantity(command.Quantity);
            }
            else
            {
                stock = new Stock(product, location, command.Quantity);
                await _stockRepository.Add(stock);
            }

            StockMovement movement = new StockMovement(stock, OperationType.Receive, command.Quantity);

            await _stockMovementRepository.Add(movement);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return stock.Id;
        }
    }
}