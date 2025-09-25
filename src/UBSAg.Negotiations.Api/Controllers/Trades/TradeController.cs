using Microsoft.AspNetCore.Mvc;
using UBSAg.Negotiations.Domain.Entities;
using UBSAg.Negotiations.Domain.Interfaces;

namespace UBSAg.Negotiations.Api.Controllers.Trades
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly ICategorizerTrades _categorizerTrades;

        public TradeController(ICategorizerTrades categorizerTrades)
        {
            _categorizerTrades = categorizerTrades;
        }

        [HttpPost("categorize")]
        public async Task<ActionResult<List<string>>> CategorizeTrades([FromBody] List<TradeRequest> tradeRequests)
        {
            var trades = tradeRequests.Select(tr => new Trade(tr.Value, tr.ClientSector)).ToList();

            var tradeCategories = await _categorizerTrades.CategorizeTradeByRisk(trades);
       
            return Ok(tradeCategories);
        }
    }
}
