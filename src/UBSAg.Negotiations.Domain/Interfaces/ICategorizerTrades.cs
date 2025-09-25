using UBSAg.Negotiations.Domain.Entities;

namespace UBSAg.Negotiations.Domain.Interfaces
{
    public interface ICategorizerTrades
    {
        Task<List<string?>> CategorizeTradeByRisk(List<Trade> trades);
    }
}
