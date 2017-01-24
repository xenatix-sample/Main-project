using Axis.Model.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Axis.Data.Repository
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        Task<Response<T>> ExecuteStoredProcAsync(string storedProcedureName, List<SqlParameter> parameters = null);
        Task<Response<T>> ExecuteNQStoredProcAsync(string storedProcedureName, List<SqlParameter> parameters = null, bool adonResult = false, bool idResult = false);
        Response<T> ExecuteStoredProc(string storedProcedureName, List<SqlParameter> parameters = null);
        Response<T> ExecuteNQStoredProc(string storedProcedureName, List<SqlParameter> parameters = null, bool adonResult = false, bool idResult = false);
    }
}
