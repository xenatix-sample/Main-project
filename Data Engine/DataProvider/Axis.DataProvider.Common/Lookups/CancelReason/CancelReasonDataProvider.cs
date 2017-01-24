using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// CancelReason Data provider
    /// </summary>
    public class CancelReasonDataProvider : ICancelReasonDataProvider
    {
        #region Class Variables

        readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public CancelReasonDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Cancel reason list
        /// </summary>
        /// <returns>Cancel reasons</returns>
        public Response<CancelReasonModel> GetCancelReasons()
        {
            var repository = _unitOfWork.GetRepository<CancelReasonModel>(SchemaName.Scheduling);
            var results = repository.ExecuteStoredProc("usp_GetCancelReason");

            return results;
        }

        #endregion
    }
}
