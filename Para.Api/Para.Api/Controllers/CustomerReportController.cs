using MediatR;
using Microsoft.AspNetCore.Mvc;
using Para.Bussiness.Cqrs;
using Para.Schema;

namespace Para.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("report")]
        public async Task<ActionResult<List<CustomerReport>>> GetCustomerReport()
        {
            try
            {
                var query = new GetCustomerReportQuery();
                var response = await _mediator.Send(query);

                if (response == null || response.Data == null || !response.Data.Any())
                {
                    return NotFound("No data found.");
                }

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
