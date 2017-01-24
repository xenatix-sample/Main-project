using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Common
{
    public class ReferralConcernTypeDataProvider : IReferralConcernTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ReferralConcernTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ReferralConcernTypeModel> GetReferralConcerns()
        {
            var repository = _unitOfWork.GetRepository<ReferralConcernTypeModel>(SchemaName.Reference);
            var procParams = new List<SqlParameter> { new SqlParameter("ProgramID", 1) };
            var results = repository.ExecuteStoredProc("usp_GetReferralConcern", procParams);

            return results;
        }

        public Response<ReferralConcernTypeModel> GetReferralProblems()
        {
            var repository = _unitOfWork.GetRepository<ReferralConcernTypeModel>(SchemaName.Reference);
            var procParams = new List<SqlParameter> { new SqlParameter("ProgramID", 2) };
            var results = repository.ExecuteStoredProc("usp_GetReferralConcern", procParams);

            return results;
        }

        #endregion exposed functionality
    }
}
