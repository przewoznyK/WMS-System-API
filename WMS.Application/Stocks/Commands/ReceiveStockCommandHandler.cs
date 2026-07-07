using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;
using WMS.Infrastructure;

namespace WMS.Application.Stocks.Commands
{
    internal class ReceiveStockCommandHandler : IRequestHandler<ReceiveStockCommand, Guid>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly WmsDbContext _wmsDbContext;

        public ReceiveStockCommandHandler(IStockRepository stockRepository, IStockMovementRepository stockMovementRepository, WmsDbContext wmsDbContext)
        {
            _stockRepository = stockRepository;
            _stockMovementRepository = stockMovementRepository;
            _wmsDbContext = wmsDbContext;
        }

        public async Task<Guid> Handle(ReceiveStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _stockRepository.GetByProductAndLocationAsync(request.ProductId, request.LocationId);

            if (stock != null)
            {
                stock.IncreaseQuantity(request.Quantity);
            }
            else
            {
                stock = new Stock(request.ProductId, request.LocationId, request.Quantity);
                await _stockRepository.AddAsync(stock);
            }

            StockMovement movement = new StockMovement(stock.ProductId, stock.LocationId, request.Quantity);
            await _stockMovementRepository.AddAsync(movement);
            await _wmsDbContext.SaveChangesAsync(cancellationToken);

            return stock.Id;
        }
    }
}