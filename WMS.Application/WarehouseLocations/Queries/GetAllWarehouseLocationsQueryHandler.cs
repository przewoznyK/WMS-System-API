using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Queries
{
    internal class GetAllWarehouseLocationsQueryHandler : IRequestHandler<GetAllWarehouseLocationsQuery, IEnumerable<WarehouseLocation>>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;

        public GetAllWarehouseLocationsQueryHandler(IWarehouseLocationRepository warehouseLocationRepository)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
        }

        public async Task<IEnumerable<WarehouseLocation>> Handle(GetAllWarehouseLocationsQuery request, CancellationToken cancellationToken)
        {
            return await _warehouseLocationRepository.GetAllAsync();
        }
    }
}