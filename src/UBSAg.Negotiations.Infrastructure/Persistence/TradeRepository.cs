using Dapper;
using UBSAg.Negotiations.Domain.Entities;
using UBSAg.Negotiations.Domain.Repositories;

namespace UBSAg.Negotiations.Infrastructure.Persistence
{
    public class TradeRepository : ITradeRepository
    {    
        private readonly IUnitOfWork _uow;
        public TradeRepository(IUnitOfWork uow) => _uow = uow;

        public async Task AddAsync(Trade trade)
        {
            var sql = @"INSERT INTO trades (Value, ClientSector)
                    VALUES (@Value, @ClientSector)";
            await _uow.Connection.ExecuteAsync(sql, trade, _uow.Transaction);
        }

        public async Task<IEnumerable<Trade>> GetAllAsync()
        {
            var sql = @"SELECT Value, ClientSector FROM trades";
            return await _uow.Connection.QueryAsync<Trade>(sql);
        }
    }
}