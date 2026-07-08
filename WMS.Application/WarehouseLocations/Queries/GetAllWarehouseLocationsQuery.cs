using MediatR;
namespace WMS.Application.WarehouseLocations.Queries
{
    public record GetAllWarehouseLocationsQuery() : IRequest<IEnumerable<string>>;
}