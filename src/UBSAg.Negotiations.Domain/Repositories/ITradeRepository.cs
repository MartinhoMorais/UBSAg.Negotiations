using UBSAg.Negotiations.Domain.Entities;

namespace UBSAg.Negotiations.Domain.Repositories
{
    public interface ITradeRepository
    {
        Task AddAsync(Trade trade);
        Task<IEnumerable<Trade>> GetAllAsync();
    }
}
