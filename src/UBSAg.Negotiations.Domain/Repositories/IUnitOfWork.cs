using System.Data;

namespace UBSAg.Negotiations.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction? Transaction { get; }
        Task BeginAsync();
        void Commit();
        void Rollback();
    }
}
