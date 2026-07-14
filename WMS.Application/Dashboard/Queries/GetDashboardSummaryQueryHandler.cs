using MediatR;
using WMS.Application.Common.Settings;
using WMS.Application.Dashboard.Queries;
using WMS.Application.Dashboard.Responses;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Application.Products.Queries
{
    internal class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IWarehouseLocationRepository _locationRepository;
        private readonly IStockMovementRepository _movementRepository;

        public GetDashboardSummaryQueryHandler(
             IProductRepository productRepository,
             IStockRepository stockRepository,
             IWarehouseLocationRepository locationRepository,
             IStockMovementRepository movementRepository)
        {
            _productRepository = productRepository;
            _stockRepository = stockRepository;
            _locationRepository = locationRepository;
            _movementRepository = movementRepository;
        }

        public async Task<DashboardSummaryResponse> Handle(
            GetDashboardSummaryQuery request,
            CancellationToken cancellationToken)
        {
            return new DashboardSummaryResponse
            {
                ProductCount = await _productRepository.GetCountAsync(cancellationToken),
                LocationCount = await _locationRepository.GetCountAsync(cancellationToken),
                StockQuantity = await _stockRepository.GetSumQuantityAsync(cancellationToken),
                TodayMovements = await _movementRepository.GetCountTodayAsync(cancellationToken),
                LowStockCount = await _stockRepository.GetLowStockCountAsync(DashboardSettings.LowStockThreshold, cancellationToken),
            };
        }
    }
}