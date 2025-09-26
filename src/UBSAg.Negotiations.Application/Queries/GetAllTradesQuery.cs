using MediatR;
using UBSAg.Negotiations.Application.Responses;
using UBSAg.Negotiations.Domain.Entities;

namespace UBSAg.Negotiations.Application.Queries
{
    public record GetAllTradesQuery() : IRequest<IEnumerable<TradesQueryResponse>>;
    
}
