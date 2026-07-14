using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;
using WMS.Domain.Enums;

namespace WMS.Application.Stocks.Commands
{
    internal class IssueStockCommandHandler : IRequestHandler<IssueStockCommand, Guid>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IssueStockCommandHandler(IStockRepository stockRepository, IStockMovementRepository stockMovementRepository, IUnitOfWork unitOfWork)
        {
            _stockRepository = stockRepository;
            _stockMovementRepository = stockMovementRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(IssueStockCommand request, CancellationToken cancellationToken)
        {
            Stock? stock = await _stockRepository.GetByProductSkuAndLocationCodeAsync(request.ProductSku, request.LocationCode, cancellationToken);

            if(stock == null)
            {
                throw new WmsNotFoundException(nameof(Stock), request.ProductSku);
            }

            stock.DecreaseQuantity(request.Quantity);
            StockMovement newStockMovement = new(stock, OperationType.Issue, -request.Quantity, request.IssueType, request.ReferenceNumber);
            await _stockMovementRepository.Add(newStockMovement);
            await _unitOfWork.SaveChangesAsync();

            return newStockMovement.Id;
        }
    }
}