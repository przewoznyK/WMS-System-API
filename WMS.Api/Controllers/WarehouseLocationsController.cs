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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateWarehouseLocationCommand command)
        {
           var productId = await _mediator.Send(command);
            return Ok(productId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteWarehouseLocationCommand(id));
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _mediator.Send(new GetAllWarehouseLocationsQuery());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var product = await _mediator.Send(new GetWarehouseLocationByIdQuery(id));
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateWarehouseLocationCommand command)
        {
            var commandWithId = command with { Id = id };

            await _mediator.Send(commandWithId);
            return NoContent();
        }
    }
}