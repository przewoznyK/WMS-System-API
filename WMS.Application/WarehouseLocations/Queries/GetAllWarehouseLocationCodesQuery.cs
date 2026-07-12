using MediatR;
namespace WMS.Application.WarehouseLocations.Queries
{
    public record GetAllWarehouseLocationCodesQuery() : IRequest<IEnumerable<string>>;
}