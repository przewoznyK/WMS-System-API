using MediatR;
using WMS.Application.StockMovements.Responses;
using WMS.Domain.Enums;
using WMS.Domain.Repositories;

namespace WMS.Application.StockMovements.Queries
{
    internal class GetStockMovementChartQueryHandler : IRequestHandler<GetStockMovementChartQuery, IEnumerable<StockMovementChartResponse>>
    {
        private readonly IStockMovementRepository _stockMovementRepository;

        public GetStockMovementChartQueryHandler(IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<IEnumerable<StockMovementChartResponse>> Handle(
    GetStockMovementChartQuery request,
    CancellationToken cancellationToken)
        {
            var toDate = DateTime.UtcNow.Date.AddDays(1);

            var fromDate = DateTime.UtcNow.Date
                .AddDays(-(request.Days - 1));


            var chartData = await _stockMovementRepository
                .GetMovementChartAsync(
                    fromDate,
                    toDate,
                    cancellationToken);


            return Enumerable.Range(0, request.Days)
                .Select(offset =>
                {
                    var date = fromDate.AddDays(offset);


                    var data = chartData.FirstOrDefault(x =>
                        x.Date == date);


                    return new StockMovementChartResponse
                    {
                        Date = date,
                        ReceiveCount = data?.ReceiveCount ?? 0,
                        IssueCount = data?.IssueCount ?? 0,
                        TransferCount = data?.TransferCount ?? 0
                    };
                });
        }
    }
}