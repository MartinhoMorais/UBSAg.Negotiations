using UBSAg.Negotiations.Domain.Entities;

namespace UBSAg.Negotiations.Domain.Interfaces
{
    public interface ICategory
    {
        void SetNext(ICategory nextCategory);

        string? Handle(Trade trade);
    }
}
