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
    public class ReferralAgencyDataProvider : IReferralAgencyDataProvider
    {
         #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ReferralAgencyDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Get eferral angency
        /// </summary>
        /// <returns></returns>
        public Response<ReferralAgencyModel> GetReferralAgency()
        {
            var repository = _unitOfWork.GetRepository<ReferralAgencyModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralAgency");

            return results;
        }

        #endregion exposed functionality
    }
}
