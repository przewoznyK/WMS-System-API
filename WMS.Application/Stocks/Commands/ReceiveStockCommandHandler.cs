using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Infrastructure;

namespace WMS.Application.Stocks.Commands
{
    internal class ReceiveStockCommandHandler : IRequestHandler<ReceiveStockCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly WmsDbContext _wmsDbContext;

        public ReceiveStockCommandHandler(IProductRepository productRepository, IWarehouseLocationRepository warehouseLocationRepository, IStockRepository stockRepository,
            IStockMovementRepository stockMovementRepository,
            WmsDbContext wmsDbContext)
        {
            _productRepository = productRepository;
            _warehouseLocationRepository = warehouseLocationRepository;
            _stockRepository = stockRepository;
            _stockMovementRepository = stockMovementRepository;
            _wmsDbContext = wmsDbContext;
        }

        public async Task<Guid> Handle(ReceiveStockCommand command, CancellationToken cancellationToken)
        {
             _wmsDbContext.StockMovements.ExecuteDelete();
            var product = await _productRepository.GetByNameAsync(command.ProductName);

            if (product == null)
            {
                throw new WmsNotFoundException("Product", command.ProductName);
            }

            var location = await _warehouseLocationRepository.GetByCodeAsync(command.LocationCode);

            if (location == null)
            {
                throw new WmsNotFoundException("WarehouseLocation", command.LocationCode);
            }

            var stock = await _stockRepository.GetByProductAndLocationAsync(product.Id, location.Id);

            if (stock != null)
            {
                stock.IncreaseQuantity(command.Quantity);
            }
            else
            {
                stock = new Stock(product.Id, location.Id, command.Quantity);
                await _stockRepository.AddAsync(stock);
            }

            StockMovement movement = new StockMovement(stock, OperationType.Receive, command.Quantity);

            await _stockMovementRepository.AddAsync(movement);
            await _wmsDbContext.SaveChangesAsync(cancellationToken);

            return stock.Id;
        }
    }
}