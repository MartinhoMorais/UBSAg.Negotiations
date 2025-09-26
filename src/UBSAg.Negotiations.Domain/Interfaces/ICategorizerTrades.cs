using UBSAg.Negotiations.Domain.Entities;

namespace UBSAg.Negotiations.Domain.Interfaces
{
    public interface ICategorizerTrades
    {
        Task<List<string?>> CategorizeTradesByRisk(List<Trade> trades);
        Task<string?> CategorizeTradeByRisk(Trade trade);
    }
}
