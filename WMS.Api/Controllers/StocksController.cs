using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Products.Commands;
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

        [HttpGet("Get all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var stocks = await _mediator.Send(new GetAllStocksQuery());
            return Ok(stocks);
        }

        [HttpPost("Move product")]
        public async Task<IActionResult> MoveProduct([FromBody] MoveProductCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("Receive")]
        public async Task<IActionResult> ReceiveStock([FromBody] ReceiveStockCommand command)
        {
            var stockId = await _mediator.Send(command);
            return Ok(stockId);
        }
    }
}