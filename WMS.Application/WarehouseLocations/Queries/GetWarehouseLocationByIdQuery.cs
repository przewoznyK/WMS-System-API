using MediatR;
using WMS.Domain.Entities;

namespace WMS.Application.WarehouseLocations.Queries
{
    public record GetWarehouseLocationByIdQuery(Guid Id) : IRequest<WarehouseLocation>;
}