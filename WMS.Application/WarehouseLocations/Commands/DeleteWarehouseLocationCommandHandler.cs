using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Commands
{
    internal class DeleteWarehouseLocationCommandHandler : IRequestHandler<DeleteWarehouseLocationCommand>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;

        public DeleteWarehouseLocationCommandHandler(IWarehouseLocationRepository warehouseLocationRepository)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
        }

        public async Task Handle(DeleteWarehouseLocationCommand request, CancellationToken cancellationToken)
        {
            var product = await _warehouseLocationRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new WmsNotFoundException(nameof(Product), request.Id);
            }

            await _warehouseLocationRepository.DeleteAsync(product);
        }
    }
}