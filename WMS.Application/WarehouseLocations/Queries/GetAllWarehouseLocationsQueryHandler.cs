using MediatR;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Queries
{
    internal class GetAllWarehouseLocationsQueryHandler : IRequestHandler<GetAllWarehouseLocationsQuery, IEnumerable<string>>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;

        public GetAllWarehouseLocationsQueryHandler(IWarehouseLocationRepository warehouseLocationRepository)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetAllWarehouseLocationsQuery request, CancellationToken cancellationToken)
        {
            return await _warehouseLocationRepository.GetLocationsAsync();
        }
    }
}