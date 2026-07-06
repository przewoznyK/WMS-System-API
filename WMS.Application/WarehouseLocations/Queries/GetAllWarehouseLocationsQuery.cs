using MediatR;
using WMS.Domain.Entities;

namespace WMS.Application.WarehouseLocations.Queries
{
    public record GetAllWarehouseLocationsQuery() : IRequest<IEnumerable<WarehouseLocation>>;
}