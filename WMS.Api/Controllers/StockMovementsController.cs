using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.StockMovements.Queries;
using WMS.Application.Stocks.Queries;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockMovementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockMovementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var stockMovements = await _mediator.Send(new GetAllStockMovementsQuery());
            return Ok(stockMovements);
        }

        [Authorize]
        [HttpGet("summary")]
        public async Task<IActionResult> GetAllViewsAsync()
        {
            var stocks = await _mediator.Send(new GetAllStockMovementsViewsQuery());
            return Ok(stocks);
        }

        [Authorize]
        [HttpGet("chart")]
        public async Task<IActionResult> GetChartAsync([FromQuery] int days = 7, CancellationToken cancellationToken = default)
        {
            var chart = await _mediator.Send(new GetStockMovementChartQuery(days), cancellationToken);
            return Ok(chart);
        }
    }
}