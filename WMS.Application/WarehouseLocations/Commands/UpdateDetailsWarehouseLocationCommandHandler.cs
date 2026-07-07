using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Commands
{
    internal class UpdateDetailsWarehouseLocationCommandHandler : IRequestHandler<UpdateDetailsWarehouseLocationCommand>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;

        public UpdateDetailsWarehouseLocationCommandHandler(IWarehouseLocationRepository warehouseLocationRepository)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
        }

        public async Task Handle(UpdateDetailsWarehouseLocationCommand request, CancellationToken cancellationToken)
        {
            var warehouseLocation = await _warehouseLocationRepository.GetWarehouseByIdAsync(request.Id);

            if (warehouseLocation == null)
            {
                throw new WmsNotFoundException(nameof(WarehouseLocation), request.Id);
            }

            await _warehouseLocationRepository.UpdateDetailsAsync(warehouseLocation, request.Code, request.Description);
        }
    }
}