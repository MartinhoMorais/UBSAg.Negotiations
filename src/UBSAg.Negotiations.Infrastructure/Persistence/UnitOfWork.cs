using Npgsql;
using System.Data;
using UBSAg.Negotiations.Domain.Repositories;

namespace UBSAg.Negotiations.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {     
        private readonly DapperContext _context;
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;
        private bool _disposed;

        public UnitOfWork(DapperContext context) => _context = context;
        public IDbConnection Connection
        {
            get
            {
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {
                    _connection = _context.CreateConnection();
                    _connection.Open();
                }
                return _connection;
            }
        }
        public IDbTransaction? Transaction => _transaction;

        public async Task BeginAsync()
        {
            if (_connection == null)
            {
                _connection = _context.CreateConnection();
            }

            if (_connection.State != ConnectionState.Open)
            {
                await ((NpgsqlConnection)_connection).OpenAsync();
            }

            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {                
                _transaction?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
            }

            _disposed = true;
        }
    }
}