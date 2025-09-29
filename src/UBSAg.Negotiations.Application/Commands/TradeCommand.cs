using MediatR;
using UBSAg.Negotiations.Application.Requests;

namespace UBSAg.Negotiations.Application.Commands
{
    public record TradeCommand(List<TradeRequest> Trades) : IRequest<List<string?>>;
}
