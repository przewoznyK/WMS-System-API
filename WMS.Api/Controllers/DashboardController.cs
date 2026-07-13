using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Dashboard.Queries;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("summary")]
        public async Task<IActionResult> GetSummaryAsync()
        {
            var dashboard = await _mediator.Send(new GetDashboardSummaryQuery());

            return Ok(dashboard);
        }
    }
}