using MediatR;

namespace WMS.Application.WarehouseLocations.Commands
{
    public record UpdateDetailsWarehouseLocationCommand(Guid Id, string Code, string Description) : IRequest;
}