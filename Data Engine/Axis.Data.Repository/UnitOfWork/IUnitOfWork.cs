using System;
using System.Collections.Generic;
using Axis.Data.Repository.Schema;
using System.Data.SqlClient;

namespace Axis.Data.Repository
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>(SchemaName schemaName = SchemaName.Core) where T : class;
        IDisposable BeginTransactionScope();
        bool IsInTransaction();
        void TransactionScopeComplete(IDisposable transactionScope);
        T EnsureInTransaction<T>(Func<string, List<SqlParameter>, bool, bool, T> action, string procedureName, List<SqlParameter> parameters, bool adonResult = false, bool idResult = false, bool forceRollback = false);
    }
}