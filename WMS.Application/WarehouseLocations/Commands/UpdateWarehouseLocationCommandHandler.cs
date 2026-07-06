using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Commands
{
    internal class UpdateWarehouseLocationCommandHandler : IRequestHandler<UpdateWarehouseLocationCommand>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;

        public UpdateWarehouseLocationCommandHandler(IWarehouseLocationRepository warehouseLocationRepository)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
        }

        public async Task Handle(UpdateWarehouseLocationCommand request, CancellationToken cancellationToken)
        {
            var warehouseLocation = await _warehouseLocationRepository.GetWarehouseByIdAsync(request.Id);

            if (warehouseLocation == null)
            {
                throw new WmsNotFoundException(nameof(Product), request.Id);
            }

            await _warehouseLocationRepository.UpdateDetailsAsync(warehouseLocation, request.Code, request.Description);
        }
    }
}