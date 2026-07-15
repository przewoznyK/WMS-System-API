using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Products.Commands;
using WMS.Application.Products.Queries;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateProductCommand command)
        {
           var productId = await _mediator.Send(command);
            return Ok(productId);
        }

        [Authorize]
        [HttpDelete("delete-{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [Authorize]
        [HttpGet("names")]
        public async Task<IActionResult> GetNamesAsync()
        {
            var products = await _mediator.Send(new GetProductNamesQuery());
            return Ok(products);
        }

        [Authorize]
        [HttpGet("by-id-{id}")]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(product);
        }

        [Authorize]
        [HttpGet("by-sku-or-name/{skuOrName}")]
        public async Task<IActionResult> GetBySkuOrNameAsync(string skuOrName)
        {
            var product = await _mediator.Send(new GetProductBySkuOrNameQuery(skuOrName));
            return Ok(product);
        }

        [Authorize]
        [HttpPut("update-details-{id}")]
        public async Task<IActionResult> UpdateDetailsAsync(Guid id, UpdateDetailsProductCommand command)
        {
            var commandWithId = command with { Id = id };

            await _mediator.Send(commandWithId);
            return NoContent();
        }

        [Authorize]
        [HttpGet("summary")]
        public async Task<IActionResult> GetAllViewsAsync()
        {
            var products = await _mediator.Send(new GetAllProductsViewsQuery());
            return Ok(products);
        }
    }
}