using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Common.Settings;
using WMS.Application.Products.Commands;
using WMS.Application.Products.Queries;
using WMS.Application.Stocks.Commands;
using WMS.Application.Stocks.Queries;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StocksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Manager,Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var stocks = await _mediator.Send(new GetAllStocksQuery());
            return Ok(stocks);
        }

        [Authorize(Roles = "Manager,Worker")]
        [HttpGet("summary")]
        public async Task<IActionResult> GetAllViewsAsync()
        {
            var stocks = await _mediator.Send(new GetAllStocksViewsQuery());
            return Ok(stocks);
        }

        [Authorize(Roles = "Manager,Worker")]
        [HttpPost("move")]
        public async Task<IActionResult> MoveProduct([FromBody] MoveStockCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "Manager,Worker")]
        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveStock([FromBody] ReceiveStockCommand command)
        {
            var stockId = await _mediator.Send(command);
            return Ok(stockId);
        }

        [Authorize(Roles = "Manager,Worker")]
        [HttpGet("by-product-sku/{sku}")]
        public async Task<IActionResult> GetAllViewsByProductSkuAsync(string sku)
        {
            var stocks = await _mediator.Send(new GetAllStocksViewsByProductSkuQuery(sku));
            return Ok(stocks);
        }

        [Authorize(Roles = "Manager,Worker")]
        [HttpPost("issue")]
        public async Task<IActionResult> IssueStock([FromBody] IssueStockCommand command)
        {
            var stockMovementId = await _mediator.Send(command);
            return Ok(stockMovementId);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("low-stock-products")]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var result = await _mediator.Send(new GetLowStockProductsQuery(DashboardSettings.LowStockThreshold));
            return Ok(result);
        }
    }
}