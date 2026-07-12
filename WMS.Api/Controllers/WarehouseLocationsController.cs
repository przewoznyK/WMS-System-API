using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateWarehouseLocationCommand command)
        {
           var locationId = await _mediator.Send(command);
            return Ok(locationId);
        }

        [HttpDelete("delete-{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteWarehouseLocationCommand(id));
            return NoContent();
        }

        [HttpGet("location-codes")]
        public async Task<IActionResult> GetCodesAsync()
        {
            var locations = await _mediator.Send(new GetAllWarehouseLocationCodesQuery());
            return Ok(locations);
        }

        [HttpGet("by-id-{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var location = await _mediator.Send(new GetWarehouseLocationByIdQuery(id));
            return Ok(location);
        }

        [HttpPut("update-details-{id}")]
        public async Task<IActionResult> UpdateDetailsAsync(Guid id, UpdateDetailsWarehouseLocationCommand command)
        {
            var commandWithId = command with { Id = id };

            await _mediator.Send(commandWithId);
            return NoContent();
        }
    }
}