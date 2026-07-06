using MediatR;

namespace WMS.Application.WarehouseLocations.Commands
{
    public record DeleteWarehouseLocationCommand(Guid Id) : IRequest;
}
