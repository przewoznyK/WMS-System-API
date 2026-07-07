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

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(CreateWarehouseLocationCommand command)
        {
           var productId = await _mediator.Send(command);
            return Ok(productId);
        }

        [HttpDelete("Delete {id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteWarehouseLocationCommand(id));
            return NoContent();
        }

        [HttpGet("Get all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _mediator.Send(new GetAllWarehouseLocationsQuery());
            return Ok(products);
        }

        [HttpGet("Get product by id {id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var product = await _mediator.Send(new GetWarehouseLocationByIdQuery(id));
            return Ok(product);
        }

        [HttpPut("Update details {id}")]
        public async Task<IActionResult> UpdateDetailsAsync(Guid id, UpdateDetailsWarehouseLocationCommand command)
        {
            var commandWithId = command with { Id = id };

            await _mediator.Send(commandWithId);
            return NoContent();
        }
    }
}