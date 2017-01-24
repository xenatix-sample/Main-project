using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Common
{
    public class ReportDataProvider : IReportDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        public ReportDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ReportsModel> GetReportsByType(string reportTypeName)
        {
            var repository = _unitOfWork.GetRepository<ReportsModel>();
            var results = repository.ExecuteStoredProc("[usp_GetReportsByType]", new List<SqlParameter>
            {
                new SqlParameter("ReportTypeName", reportTypeName ),
                new SqlParameter("UserID", AuthContext.Auth.User.UserID )
            });
            return results;
        }

        #endregion exposed functionality
    }
}