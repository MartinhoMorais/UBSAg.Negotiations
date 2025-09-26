using UBSAg.Negotiations.Domain.Entities;
using UBSAg.Negotiations.Domain.Interfaces;

namespace UBSAg.Negotiations.Application.Categories
{
    public class CategorizerTrades : ICategorizerTrades
    {
        private readonly ICategory _riskChain;

        public CategorizerTrades(ICategory riskChain)
        {
            _riskChain = riskChain;
        }

        public async Task<List<string?>> CategorizeTradesByRisk(List<Trade> trades)
        {            
            List<string?> tradeCategories = [];

            foreach (var trade in trades)
            {
                tradeCategories.Add(_riskChain.Handle(trade));
            }

            return await Task.FromResult(tradeCategories);
        }

        public async Task<string?> CategorizeTradeByRisk(Trade trade)
        {
            return await Task.FromResult(_riskChain.Handle(trade));
        }
    }
}
