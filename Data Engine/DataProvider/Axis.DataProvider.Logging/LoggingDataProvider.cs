using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Logging
{
    public class LoggingDataProvider : ILoggingDataProvider
    {
        IUnitOfWork unitOfWork = null;
        public LoggingDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void LogException(ExceptionModel exception)
        {
            try
            {
                var exceptionRepository = unitOfWork.GetRepository<ExceptionModel>(SchemaName.Core);

                SqlParameter messageParam = new SqlParameter("Message", exception.Message ?? string.Empty);
                SqlParameter sourceParam = new SqlParameter("Source", exception.Source ?? string.Empty);
                SqlParameter commentsParam = new SqlParameter("Comments", exception.Comments ?? string.Empty);
                SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", exception.ModifiedOn ?? DateTime.Now);
                List<SqlParameter> procParams = new List<SqlParameter>() { messageParam, sourceParam, commentsParam, modifiedOnParam };

                exceptionRepository.ExecuteNQStoredProc("usp_LogException", procParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LogActivity(ActivityModel activity)
        {
            throw new NotImplementedException();
        }
    }
}
