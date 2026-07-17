using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Products.Request;
using WMS.Application.WarehouseLocations.Commands;
using WMS.Application.WarehouseLocations.Queries;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseLocationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WarehouseLocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateWarehouseLocationCommand command)
        {
            var locationId = await _mediator.Send(command);
            return Ok(locationId);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("delete-{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteWarehouseLocationCommand(id));
            return NoContent();
        }

        [Authorize(Roles = "Manager, Worker")]
        [HttpGet("location-codes")]
        public async Task<IActionResult> GetCodesAsync()
        {
            var locations = await _mediator.Send(new GetAllWarehouseLocationCodesQuery());
            return Ok(locations);
        }

        [Authorize(Roles = "Manager, Worker")]
        [HttpGet("by-id-{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var location = await _mediator.Send(new GetWarehouseLocationByIdQuery(id));
            return Ok(location);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("update-details-{id}")]
        public async Task<IActionResult> UpdateDetailsAsync(Guid id, UpdateDetailsWarehouseLocationRequest request)
        {
            var command = new UpdateDetailsWarehouseLocationCommand(id, request.LocationCode, request.Description ?? "");
            
            await _mediator.Send(command);
            return NoContent();
        }
    }
}