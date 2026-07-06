using MediatR;

namespace WMS.Application.WarehouseLocations.Commands
{
    public record UpdateWarehouseLocationCommand(Guid Id, string Code, string Description) : IRequest;
}