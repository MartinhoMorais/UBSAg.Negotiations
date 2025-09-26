using MediatR;
using UBSAg.Negotiations.Application.Commands;
using UBSAg.Negotiations.Domain.Entities;
using UBSAg.Negotiations.Domain.Interfaces;
using UBSAg.Negotiations.Domain.Repositories;

namespace UBSAg.Negotiations.Application.Handlers
{
    public class TradeHandler : IRequestHandler<TradeCommand, List<string?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITradeRepository _tradeRepository;
        private readonly ICategorizerTrades _categorizerTrades;

        public TradeHandler(IUnitOfWork unitOfWork, ITradeRepository tradeRepository, ICategorizerTrades categorizerTrades )
        {
            _unitOfWork = unitOfWork;
            _tradeRepository = tradeRepository;
            _categorizerTrades = categorizerTrades;
        }

        public async Task<List<string?>> Handle(TradeCommand command, CancellationToken cancellationToken)
        {
            var tradeCategories = new List<string?>();

            try
            {
                var trades = command.Trades.Select(tr => new Trade(tr.Value, tr.ClientSector)).ToList();
                 tradeCategories = await _categorizerTrades.CategorizeTradesByRisk(trades);

                await _unitOfWork.BeginAsync();

                foreach (var tr in trades)
                {
                    var trade = new Trade(tr.Value, tr.ClientSector);
                    await _tradeRepository.AddAsync(trade);
                }

                _unitOfWork.Commit();
                return tradeCategories;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
