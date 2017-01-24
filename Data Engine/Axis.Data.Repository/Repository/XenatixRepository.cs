using Axis.Data.Repository.Schema;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Data.Repository
{
    class XenatixRepository<T> : IRepository<T> where T : class
    {
        private ILogger _logger = new Logger();

        public DbContext xenatixContext;
        public DbSet<T> xenatixDbSet;
        public SchemaName SchemaName;

        public XenatixRepository(DbContext dbContext, SchemaName schemaName = SchemaName.Core)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            xenatixContext = new DbContext("XenatixDBConnection");
            xenatixDbSet = xenatixContext.Set<T>();
            this.SchemaName = schemaName;
        }

        public IQueryable<T> GetAll()
        {
            return xenatixDbSet;
        }

        public T GetById(int id)
        {
            return xenatixDbSet.Find(id);
        }

        public void Add(T entity)
        {
            var entityEntry = xenatixContext.Entry(entity);
            if (entityEntry.State != EntityState.Detached)
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                xenatixDbSet.Add(entity);
            }
        }

        public void Update(T entity)
        {
            var entityEntry = xenatixContext.Entry(entity);
            if (entityEntry.State == EntityState.Detached)
            {
                xenatixDbSet.Attach(entity);
            }

            entityEntry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            var entityEntry = xenatixContext.Entry(entity);
            if (entityEntry.State != EntityState.Deleted)
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                xenatixDbSet.Attach(entity);
                xenatixDbSet.Remove(entity);
            }
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public Response<T> ExecuteStoredProc(string storedProcedureName, List<SqlParameter> parameters = null)
        {
            Response<T> response;

            if (parameters == null)
                parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(storedProcedureName))
            {
                //Add the default output parameters
                SqlParameter resultCodeParam = new SqlParameter("ResultCode", SqlDbType.Int);
                resultCodeParam.Direction = ParameterDirection.Output;
                SqlParameter resultMessageParam = new SqlParameter("ResultMessage", SqlDbType.NVarChar, 500);
                resultMessageParam.Direction = ParameterDirection.Output;
                parameters.Add(resultCodeParam);
                parameters.Add(resultMessageParam);

                StringBuilder sb = GenerateSQL(storedProcedureName, parameters);

                try
                {
                    var list = xenatixContext.Database.SqlQuery<T>(sb.ToString(), parameters.Cast<object>().ToArray()).ToList();

                    //Handle the output parameters and populate the returning model
                    response = new Response<T>
                    {
                        DataItems = list,
                        ResultCode = (int)resultCodeParam.Value,
                        ResultMessage = resultMessageParam.Value.ToString()
                    };
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                    response = new Response<T>
                    {
                        ResultCode = -1,
                        ResultMessage = "The stored procedure failed to execute"
                    };
                }
                finally
                {
                    xenatixContext.Dispose();
                }
            }
            else
            {
                response = new Response<T>
                {
                    ResultCode = -1,
                    ResultMessage = "The stored procedure name was not provided!"
                };
            }

            return response;
        }

        async public Task<Response<T>> ExecuteStoredProcAsync(string storedProcedureName, List<SqlParameter> parameters = null)
        {
            Response<T> response;

            if (parameters == null)
                parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(storedProcedureName))
            {
                //Add the default output parameters
                SqlParameter resultCodeParam = new SqlParameter("ResultCode", SqlDbType.Int);
                resultCodeParam.Direction = ParameterDirection.Output;
                SqlParameter resultMessageParam = new SqlParameter("ResultMessage", SqlDbType.NVarChar, 500);
                resultMessageParam.Direction = ParameterDirection.Output;
                parameters.Add(resultCodeParam);
                parameters.Add(resultMessageParam);

                StringBuilder sb = GenerateSQL(storedProcedureName, parameters);

                try
                {
                    var list =
                        await xenatixContext.Database.SqlQuery<T>(sb.ToString(), parameters.Cast<object>().ToArray())
                            .ToListAsync();

                    //Handle the output parameters and populate the returning model
                    response = new Response<T>
                    {
                        DataItems = list,
                        ResultCode = (int)resultCodeParam.Value,
                        ResultMessage = resultMessageParam.Value.ToString()
                    };
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                    response = new Response<T>
                    {
                        ResultCode = -1,
                        ResultMessage = "The stored procedure failed to execute"
                    };
                }
                finally
                {
                    xenatixContext.Dispose();
                }
            }
            else
            {
                response = new Response<T>
                {
                    ResultCode = -1,
                    ResultMessage = "The stored procedure name was not provided!"
                };
            }

            return response;
        }

        public Response<T> ExecuteNQStoredProc(string storedProcedureName,
            List<SqlParameter> parameters = null,
            bool adonResult = false, bool idResult = false)
        {
            Response<T> response = null;

            if (parameters == null)
            {
                parameters = new List<SqlParameter>();
            }

            if (!string.IsNullOrEmpty(storedProcedureName))
            {
                //Add modifedby parameter
                SqlParameter modifiedByParam = new SqlParameter("ModifiedBy", AuthContext.Auth.User.UserID);
                modifiedByParam.Direction = ParameterDirection.Input;
                parameters.Add(modifiedByParam);
                //Add the default output parameters
                SqlParameter resultCodeParam = new SqlParameter("ResultCode", SqlDbType.Int);
                resultCodeParam.Direction = ParameterDirection.Output;
                SqlParameter resultMessageParam = new SqlParameter("ResultMessage", SqlDbType.NVarChar, 500);
                resultMessageParam.Direction = ParameterDirection.Output;
                SqlParameter resultIDParam = new SqlParameter("ID", SqlDbType.Int);
                resultIDParam.Direction = ParameterDirection.Output;
                SqlParameter resultAdonParam = new SqlParameter("AdditionalResult", SqlDbType.Xml);
                resultAdonParam.Direction = ParameterDirection.Output;

                parameters.Add(resultCodeParam);
                parameters.Add(resultMessageParam);

                //ID parameter only when it is required
                if (idResult)
                    parameters.Add(resultIDParam);

                //Pass additional parameter only when it is required
                if (adonResult)
                    parameters.Add(resultAdonParam);

                StringBuilder sb = GenerateSQL(storedProcedureName, parameters);

                try
                {
                    int rowsAffected = xenatixContext.Database.ExecuteSqlCommand(sb.ToString(), parameters.Cast<object>().ToArray());

                    //Handle the output parameters and populate the returning model.
                    //Add the remaining output parameters for needed for the default response model
                    response = new Response<T>
                    {
                        ResultCode = (int)resultCodeParam.Value,
                        ResultMessage = resultMessageParam.Value.ToString(),
                        RowAffected = rowsAffected,
                        ID = idResult ? Convert.ToInt32(resultIDParam.Value) : default(int),
                        AdditionalResult = adonResult ? resultAdonParam.Value.ToString() : default(string)
                    };
                }
                catch (Exception ex)
                {
                    var sParams = string.Join(", ", parameters.Select(p => string.Format("{0}: {1}", p.ParameterName, p.Value)));
                    _logger.Error(string.Format("{0} || Parameters: ({1}) || {2}", 
                        storedProcedureName, sParams, ex.Message), ex);

                    response = new Response<T>
                    {
                        ResultCode = -1,
                        ResultMessage = "The stored procedure failed to execute"
                    };
                }
            }
            else
            {
                _logger.Error("The stored procedure name was not provided!");

                response = new Response<T>
                {
                    ResultCode = -1,
                    ResultMessage = "The stored procedure name was not provided!",
                    RowAffected = 0
                };
            }

            return response;
        }

        async public Task<Response<T>> ExecuteNQStoredProcAsync(string storedProcedureName, List<SqlParameter> parameters = null,
            bool adonResult = false, bool idResult = false)
        {
            Response<T> response = null;

            if (parameters == null)
            {
                parameters = new List<SqlParameter>();
            }

            if (!string.IsNullOrEmpty(storedProcedureName))
            {
                //Add modifedby parameter
                SqlParameter modifiedByParam = new SqlParameter("ModifiedBy", AuthContext.Auth.User.UserID);
                modifiedByParam.Direction = ParameterDirection.Input;
                parameters.Add(modifiedByParam);
                //Add the default output parameters
                SqlParameter resultCodeParam = new SqlParameter("ResultCode", SqlDbType.Int);
                resultCodeParam.Direction = ParameterDirection.Output;
                SqlParameter resultMessageParam = new SqlParameter("ResultMessage", SqlDbType.NVarChar, 500);
                resultMessageParam.Direction = ParameterDirection.Output;
                SqlParameter resultIDParam = new SqlParameter("ID", SqlDbType.Int);
                resultIDParam.Direction = ParameterDirection.Output;
                SqlParameter resultAdonParam = new SqlParameter("AdditionalResult", SqlDbType.Xml);
                resultAdonParam.Direction = ParameterDirection.Output;

                parameters.Add(resultCodeParam);
                parameters.Add(resultMessageParam);

                //ID parameter only when it is required
                if (idResult)
                {
                    parameters.Add(resultIDParam);
                }

                //Pass additional parameter only when it is required
                if (adonResult)
                {
                    parameters.Add(resultAdonParam);
                }

                StringBuilder sb = GenerateSQL(storedProcedureName, parameters);

                try
                {
                    int rowsAffected = await xenatixContext.Database.ExecuteSqlCommandAsync(sb.ToString(), parameters.Cast<object>().ToArray());

                    //Handle the output parameters and populate the returning model.
                    //Add the remaining output parameters for needed for the default response model
                    response = new Response<T>
                    {
                        ResultCode = (int)resultCodeParam.Value,
                        ResultMessage = resultMessageParam.Value.ToString(),
                        RowAffected = rowsAffected,
                        ID = idResult ? Convert.ToInt32(resultIDParam.Value) : default(int),
                        AdditionalResult = adonResult ? resultAdonParam.Value.ToString() : default(string)
                    };
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                    response = new Response<T>
                    {
                        ResultCode = -1,
                        ResultMessage = "The stored procedure failed to execute"
                    };
                }
            }
            else
            {
                response = new Response<T>
                {
                    ResultCode = -1,
                    ResultMessage = "The stored procedure name was not provided!",
                    RowAffected = 0
                };
            }

            return response;
        }


        #region Private Methods

        private StringBuilder GenerateSQL(string storedProcedureName, List<SqlParameter> parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Exec ");
            if (!string.IsNullOrEmpty(this.SchemaName.GetSchemaName()))
                sb.AppendFormat("[{0}].", this.SchemaName.GetSchemaName());
            sb.Append(storedProcedureName);
            bool paramsAdded = false;

            foreach (SqlParameter item in parameters)
            {
                if (paramsAdded == false)
                {
                    sb.Append(" @");
                    paramsAdded = true;
                }
                else
                {
                    sb.Append(", @");
                }
                sb.Append(item.ParameterName);
                if (item.Direction == System.Data.ParameterDirection.Output)
                {
                    sb.Append(" OUT");
                }
            }

            return sb;
        }

        #endregion
    }
}
