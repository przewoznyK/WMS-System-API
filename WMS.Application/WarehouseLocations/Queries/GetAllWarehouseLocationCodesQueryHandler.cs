using MediatR;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Queries
{
    internal class GetAllWarehouseLocationCodesQueryHandler : IRequestHandler<GetAllWarehouseLocationCodesQuery, IEnumerable<string>>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;

        public GetAllWarehouseLocationCodesQueryHandler(IWarehouseLocationRepository warehouseLocationRepository)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetAllWarehouseLocationCodesQuery request, CancellationToken cancellationToken)
        {
            return await _warehouseLocationRepository.GetLocationsAsync();
        }
    }
}