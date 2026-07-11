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

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateProductCommand command)
        {
           var productId = await _mediator.Send(command);
            return Ok(productId);
        }

        [HttpDelete("delete-{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpGet("names")]
        public async Task<IActionResult> GetNamesAsync()
        {
            var products = await _mediator.Send(new GetProductNamesQuery());
            return Ok(products);
        }

        [HttpGet("by-id-{id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(product);
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> SearchProduct(string searchTerm)
        {
            var product = await _mediator.Send(new GetProductViewBySearchQuery(searchTerm));
            return Ok(product);
        }

        [HttpPut("update-details-{id}")]
        public async Task<IActionResult> UpdateDetailsAsync(Guid id, UpdateDetailsProductCommand command)
        {
            var commandWithId = command with { Id = id };

            await _mediator.Send(commandWithId);
            return NoContent();
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetAllViewsAsync()
        {
            var products = await _mediator.Send(new GetAllProductsViewsQuery());
            return Ok(products);
        }
    }
}