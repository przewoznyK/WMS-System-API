using MediatR;
using WMS.Domain.Entities;
using WMS.Domain.Exceptions;
using WMS.Domain.Repositories;

namespace WMS.Application.WarehouseLocations.Commands
{
    internal class CreateWarehouseLocationCommandHandler : IRequestHandler<CreateWarehouseLocationCommand, Guid>
    {
        private readonly IWarehouseLocationRepository _warehouseLocationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWarehouseLocationCommandHandler(IWarehouseLocationRepository warehouseLocationRepository, IUnitOfWork unitOfWork)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateWarehouseLocationCommand request, CancellationToken cancellationToken)
        {
            var isCodeTaken = await _warehouseLocationRepository.ExistsByCodeAsync(request.Code);

            if(isCodeTaken)
            {
                throw new WmsAlreadyExistsException("Location", nameof(request.Code), request.Code);
            }

            WarehouseLocation newWarehouseLocation = new(request.Code, request.Description);
            await _warehouseLocationRepository.Add(newWarehouseLocation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newWarehouseLocation.Id;
        }
    }
}