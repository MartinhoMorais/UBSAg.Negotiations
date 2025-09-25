using UBSAg.Negotiations.Domain.Entities;
using UBSAg.Negotiations.Domain.Interfaces;

namespace UBSAg.Negotiations.Application.Categories
{
    public class NoCategory : ICategory
    {
        private ICategory? _nextCategory;

        public void SetNext(ICategory nextCategory)
        {
            _nextCategory = nextCategory;
        }

        public string Handle(Trade trade)
        {
            return "";
        }
    }
}
