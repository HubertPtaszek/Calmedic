using Calmedic.EntityFramework;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Calmedic.Application
{
    public class DbSession
    {      
        public MainDatabaseContext MainDatabaseContext { get; set; }
        private IDbContextTransaction _transaction = null;
        private string _interceptionId;

        public DbSession()
        { }

        public void SetTransaction(string id)
        {
            if (_interceptionId == null)
            {
                _interceptionId = id;
                _transaction = MainDatabaseContext.Database.BeginTransaction();
            }
        }

        public async Task CommitTransactionAsync(string id)
        {
            if (_interceptionId == id)
            {
                _interceptionId = null;
                int data = await MainDatabaseContext.SaveChangesAsync();
                if (_transaction != null)
                {
                    _transaction.Commit();
                    _transaction = null;
                }
            }
        }

        public void CommitTransaction(string id)
        {
            if (_interceptionId == id)
            {
                _interceptionId = null;
                MainDatabaseContext.SaveChanges();
                if (_transaction != null)
                {
                    _transaction.Commit();
                    _transaction = null;
                }
            }
        }

        public void RollbackTransaction(string id)
        {
            if (_interceptionId == id)
            {
                _interceptionId = null;
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction = null;
                }
            }
        }
    }
}
