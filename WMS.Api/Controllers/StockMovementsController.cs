using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.StockMovements.Queries;

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

        [HttpGet("Get all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var stocks = await _mediator.Send(new GetAllStockMovementsQuery());
            return Ok(stocks);
        }
    }
}