using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Queries
{
    internal class GetWarehouseLocationByIdQueryHandler : IRequestHandler<GetWarehouseLocationByIdQuery, WarehouseLocation>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;

        public GetWarehouseLocationByIdQueryHandler(IWarehouseLocationRepository warehouseLocationRepository)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
        }

        public async Task<WarehouseLocation> Handle(GetWarehouseLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var warehouseLocation = await _warehouseLocationRepository.GetWarehouseByIdAsync(request.Id);

            if (warehouseLocation == null)
            {
                throw new WmsNotFoundException(nameof(Product), request.Id);
            }

            return warehouseLocation;
        }
    }
}