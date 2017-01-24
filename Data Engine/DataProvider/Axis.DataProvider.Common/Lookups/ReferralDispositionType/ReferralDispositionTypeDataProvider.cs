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
    /// <summary>
    /// 
    /// </summary>
    /// <see also cref="Axis.DataProvider.Common.IReferralDispositionTypeDataProvider" />
    public class ReferralDispositionTypeDataProvider : IReferralDispositionTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDispositionTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralDispositionTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the type of the referral disposition.
        /// </summary>
        /// <returns></returns>
        public Response<ReferralDispositionTypeModel> GetReferralDispositionType()
        {
            var repository = _unitOfWork.GetRepository<ReferralDispositionTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralDispositions");

            return results;
        }

        #endregion exposed functionality
    }
}
