using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Products.Commands;
using WMS.Application.Products.Queries;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(CreateProductCommand command)
        {
           var productId = await _mediator.Send(command);
            return Ok(productId);
        }

        [HttpDelete("Delete {id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }

        [HttpGet("Get all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpGet("Get by id {id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(product);
        }

        [HttpPut("Update details {id}")]
        public async Task<IActionResult> UpdateDetailsAsync(Guid id, UpdateDetailsProductCommand command)
        {
            var commandWithId = command with { Id = id };

            await _mediator.Send(commandWithId);
            return NoContent();
        }
    }
}