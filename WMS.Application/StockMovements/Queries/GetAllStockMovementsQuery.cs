using MediatR;
using WMS.Domain.Entities;

namespace WMS.Application.StockMovements.Queries
{
    public record GetAllStockMovementsQuery() : IRequest<IEnumerable<StockMovement>>;
}