using MediatR;
using WMS.Application.StockMovements.Responses;
using WMS.Domain.Enums;
using WMS.Domain.Repositories;

namespace WMS.Application.StockMovements.Queries
{
    internal class GetStockMovementChartQueryHandler : IRequestHandler<GetStockMovementChartQuery, IEnumerable<MovementChartResponse>>
    {
        private readonly IStockMovementRepository _stockMovementRepository;

        public GetStockMovementChartQueryHandler(IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<IEnumerable<MovementChartResponse>> Handle(GetStockMovementChartQuery request, CancellationToken cancellationToken)
        {
            var toDate = DateTime.UtcNow.Date.AddDays(1);

            var fromDate = DateTime.UtcNow.Date
                .AddDays(-(request.Days - 1));

            var movements = await _stockMovementRepository.GetBetweenDatesAsync(fromDate, toDate, cancellationToken);

            return Enumerable
                .Range(0, request.Days)
                .Select(offset =>
                {
                    var date = fromDate.AddDays(offset);

                    var dayMovements = movements
                        .Where(x => x.CreatedAt.Date == date);

                    return new MovementChartResponse
                    {
                        Date = date,

                        Receive = dayMovements.Count(x =>
                            x.OperationType == OperationType.Receive),

                        Issue = dayMovements.Count(x =>
                            x.OperationType == OperationType.Issue),

                        Transfer = dayMovements.Count(x =>
                            x.OperationType == OperationType.Transfer)
                    };
                });
        }
    }
}