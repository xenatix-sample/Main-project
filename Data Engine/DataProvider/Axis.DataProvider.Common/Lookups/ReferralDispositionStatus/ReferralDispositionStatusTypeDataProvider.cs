using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Common
{
    public class ReferralDispositionStatusTypeDataProvider : IReferralDispositionStatusTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDispositionStatusTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralDispositionStatusTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the type of the referral disposition status.
        /// </summary>
        /// <returns></returns>
        public Response<ReferralDispositionStatusTypeModel> GetReferralDispositionStatusType()
        {
            var repository = _unitOfWork.GetRepository<ReferralDispositionStatusTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralDispositionsStatus");

            return results;
        }

        #endregion exposed functionality
    }
}
