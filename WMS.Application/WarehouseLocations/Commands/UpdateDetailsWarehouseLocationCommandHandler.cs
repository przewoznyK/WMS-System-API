using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Commands
{
    internal class UpdateDetailsWarehouseLocationCommandHandler : IRequestHandler<UpdateDetailsWarehouseLocationCommand>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDetailsWarehouseLocationCommandHandler(IWarehouseLocationRepository warehouseLocationRepository, IUnitOfWork unitOfWork)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateDetailsWarehouseLocationCommand request, CancellationToken cancellationToken)
        {
            var warehouseLocation = await _warehouseLocationRepository.GetByIdAsync(request.Id);

            if (warehouseLocation == null)
            {
                throw new WmsNotFoundException(nameof(WarehouseLocation), request.Id);
            }

            warehouseLocation.UpdateDetails(request.Code, request.Description);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}