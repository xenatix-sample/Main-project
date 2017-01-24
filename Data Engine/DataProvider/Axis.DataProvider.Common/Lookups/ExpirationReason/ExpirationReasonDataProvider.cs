using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExpirationReasonDataProvider : IExpirationReasonDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ExpirationReasonDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the expiration reasons.
        /// </summary>
        /// <returns></returns>
        public Response<ExpirationReasonModel> GetExpirationReasons()
        {
            var repository = _unitOfWork.GetRepository<ExpirationReasonModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetExpirationReasonDetails");

            return results;
        }

        /// <summary>
        /// Gets the assessment expiration reasons.
        /// </summary>
        /// <returns></returns>
        public Response<ExpirationReasonModel> GetAssessmentExpirationReasons()
        {
            var repository = _unitOfWork.GetRepository<ExpirationReasonModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetAssessmentExpirationReasons");

            return results;
        }


        public Response<ExpirationReasonModel> GetOtherIDExpirationReasons()
        {
            var repository = _unitOfWork.GetRepository<ExpirationReasonModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetOtherIDExpirationReasonDetails");

            return results;
        }
        #endregion exposed functionality

    }

}
