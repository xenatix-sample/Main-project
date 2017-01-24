using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Transactions;
using Axis.Data.Repository.Schema;

namespace Axis.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext xenatixContext { get; set; }

        public UnitOfWork(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            xenatixContext = dbContext;
        }

        public IRepository<T> GetRepository<T>(SchemaName schemaName = SchemaName.Core) where T : class
        {
            return new XenatixRepository<T>(xenatixContext, schemaName);
        }

        public IDisposable BeginTransactionScope()
        {
            return new TransactionScope();
        }

        public bool IsInTransaction()
        {
            return Transaction.Current != null;
        }

        public T EnsureInTransaction<T>(Func<string, List<SqlParameter>, bool, bool, T> action, string procedureName, List<SqlParameter> parameters, bool adonResult = false, bool idResult = false, bool forceRollback = false)
        {
            T returnValue;
            if (IsInTransaction())
                returnValue = action(procedureName, parameters, adonResult, idResult);
            else
            {
                using (var transactionScope = BeginTransactionScope())
                {
                    returnValue = action(procedureName, parameters, adonResult, idResult);
                    if (!forceRollback)
                        TransactionScopeComplete(transactionScope);
                }
            }
            return returnValue;
        }

        public void TransactionScopeComplete(IDisposable transactionScope)
        {
            ((TransactionScope)transactionScope).Complete();
            xenatixContext.Dispose();
        }
    }
}
