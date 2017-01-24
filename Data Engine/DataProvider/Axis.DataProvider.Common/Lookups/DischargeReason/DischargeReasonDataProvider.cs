using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Common.Lookups;
using System;

namespace Axis.DataProvider.Common
{
    public class DischargeReasonDataProvider : IDischargeReasonDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DischargeReasonDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DischargeReasonDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        /// <summary>
        /// Gets the discharge reason.
        /// </summary>
        /// <returns></returns>
        public Response<DischargeReasonModel> GetDischargeReason()
        {
            var repository = _unitOfWork.GetRepository<DischargeReasonModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetDischargeReasonDetails");
            return results;
        }
    }
}
