using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.Assessment;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Common.Lookups.Assesssment
{
    public class AssessmentDataProvider : IAssessmentDataProvider
    {


        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public AssessmentDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations


        #region exposed functionality
        /// <summary>
        /// Get Assessment
        /// </summary>
        /// <returns></returns>
        public Response<AssessmentModel> GetAssessment(int documentTypeID)
        {
            var repository = _unitOfWork.GetRepository<AssessmentModel>(SchemaName.Core);
            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("DocumentTypeID", documentTypeID));
            var results = repository.ExecuteStoredProc("usp_GetAssessmentNames", param);
            return results;
        }

        public Response<AssessmentModel> GetAssessmentList()
        {
            var repository = _unitOfWork.GetRepository<AssessmentModel>(SchemaName.Core);
            var param = new List<SqlParameter>();
            var results = repository.ExecuteStoredProc("usp_GetAssessmentsList", param); 
            return results;
        }

        #endregion exposed functionality
    }
}
