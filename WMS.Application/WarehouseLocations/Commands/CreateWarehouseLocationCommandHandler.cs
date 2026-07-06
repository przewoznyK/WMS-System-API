using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Commands
{
    internal class CreateWarehouseLocationCommandHandler : IRequestHandler<CreateWarehouseLocationCommand, Guid>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;

        public CreateWarehouseLocationCommandHandler(IWarehouseLocationRepository warehouseLocationRepository)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
        }

        public async Task<Guid> Handle(CreateWarehouseLocationCommand request, CancellationToken cancellationToken)
        {
            WarehouseLocation newWarehouseLocation = new(request.Code, request.Description);
            await _warehouseLocationRepository.AddAsync(newWarehouseLocation);

            return newWarehouseLocation.Id;
        }
    }
}