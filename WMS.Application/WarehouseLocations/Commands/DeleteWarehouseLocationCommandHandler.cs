using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Commands
{
    internal class DeleteWarehouseLocationCommandHandler : IRequestHandler<DeleteWarehouseLocationCommand>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWarehouseLocationCommandHandler(IWarehouseLocationRepository warehouseLocationRepository, IUnitOfWork unitOfWork)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteWarehouseLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await _warehouseLocationRepository.GetByIdAsync(request.Id);

            if (location == null)
            {
                throw new WmsNotFoundException(nameof(WarehouseLocation), request.Id);
            }

            await _warehouseLocationRepository.Delete(location);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}