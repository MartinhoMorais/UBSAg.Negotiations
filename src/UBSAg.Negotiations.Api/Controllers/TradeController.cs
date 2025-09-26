using MediatR;
using Microsoft.AspNetCore.Mvc;
using UBSAg.Negotiations.Application.Commands;
using UBSAg.Negotiations.Application.Queries;
using UBSAg.Negotiations.Application.Requests;
using UBSAg.Negotiations.Domain.Entities;

namespace UBSAg.Negotiations.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TradeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<List<string>>> CategorizeTrades([FromBody] List<TradeRequest> tradeRequests)
        {
            var result = await _mediator.Send(new TradeCommand(tradeRequests));

            return Ok(result);
        }

        [HttpGet("categorize")]
        public async Task<ActionResult<IEnumerable<Trade>>> GetAllTrades()
        {
            var trades = await _mediator.Send(new GetAllTradesQuery());

            return Ok(trades);
        }
    }
}
