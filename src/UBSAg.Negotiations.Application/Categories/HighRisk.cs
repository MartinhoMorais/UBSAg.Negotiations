using UBSAg.Negotiations.Domain.Constants;
using UBSAg.Negotiations.Domain.Entities;
using UBSAg.Negotiations.Domain.Enums;
using UBSAg.Negotiations.Domain.Interfaces;

namespace UBSAg.Negotiations.Application.Categories
{
    public class HighRisk : ICategory
    {
        private ICategory? _nextCategory;

        public void SetNext(ICategory nextCategory)
        {
            _nextCategory = nextCategory;
        }

        public string? Handle(Trade trade)
        {
            if (trade.Value > RiskValueConstant.RISK_CUTOFF_VALUE &&
                trade.ClientSector.Equals(ClientSectorConstant.PRIVATE_SECTOR.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return TradeCategoriesEnum.HighRisk.ToString().ToUpper();
            }

            return _nextCategory?.Handle(trade);
        }
    }
}
