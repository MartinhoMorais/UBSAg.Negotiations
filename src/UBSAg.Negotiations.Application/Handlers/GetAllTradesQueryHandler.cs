using MediatR;
using UBSAg.Negotiations.Application.Queries;
using UBSAg.Negotiations.Application.Responses;
using UBSAg.Negotiations.Domain.Interfaces;
using UBSAg.Negotiations.Domain.Repositories;

namespace UBSAg.Negotiations.Application.Handlers
{
    public class GetAllTradesQueryHandler : IRequestHandler<GetAllTradesQuery, IEnumerable<TradesQueryResponse>>
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly ICategorizerTrades _categorizerTrades;

        public GetAllTradesQueryHandler(ITradeRepository tradeRepository, ICategorizerTrades categorizerTrades)
        {
            _tradeRepository=tradeRepository;
            _categorizerTrades=categorizerTrades;
        }

        public async Task<IEnumerable<TradesQueryResponse>> Handle(GetAllTradesQuery request, CancellationToken cancellationToken)
        {
            var trades = await _tradeRepository.GetAllAsync();

            if (trades == null || !trades.Any())
                return [];

            var responseTasks = trades.Select(async tr => new TradesQueryResponse
            {
                Value = tr.Value,
                ClientSector = tr.ClientSector,
                TradeCategory = await _categorizerTrades.CategorizeTradeByRisk(tr)
            });

            var responses = await Task.WhenAll(responseTasks);

            return responses;

        }
    }
}
