using MediatR;

namespace WMS.Application.WarehouseLocations.Commands
{
    public record CreateWarehouseLocationCommand(string Code, string? Description) : IRequest<Guid>;
}