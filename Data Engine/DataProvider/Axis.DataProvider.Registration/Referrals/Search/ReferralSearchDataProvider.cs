using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Registration.Referrals
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Registration.Referrals.IReferralSearchDataProvider" />
    public class ReferralSearchDataProvider : IReferralSearchDataProvider
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSearchDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralSearchDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the referrals.
        /// </summary>
        /// <param name="searchStr">The string to search.</param>
        /// <returns></returns>
        public Response<ReferralSearchModel> GetReferrals(string searchStr, int searchType, long userID)
        {
            var procParams = new List<SqlParameter>();
            procParams.Add(new SqlParameter("SearchCriteria", (object)searchStr ?? DBNull.Value));
            procParams.Add(new SqlParameter("UserID", userID));
            procParams.Add(new SqlParameter("View", searchType));
            var referralRepository = _unitOfWork.GetRepository<ReferralSearchModel>(SchemaName.Registration);
            return referralRepository.ExecuteStoredProc("usp_GetIncomingOutgoingReferrals", procParams);
        }

        /// <summary>
        /// Deletes the referral.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reasonForDelete"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralSearchModel> DeleteReferral(long id, string reasonForDelete, DateTime modifiedOn)
        {
            reasonForDelete = string.IsNullOrWhiteSpace(reasonForDelete) ? "" : reasonForDelete; //TODO: remove this line after making sure reason for delete is never blank
            var referralRepository = _unitOfWork.GetRepository<ReferralSearchModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { 
                                                new SqlParameter("ReferralHeaderID", id),
                                                new SqlParameter("ReasonForDelete", reasonForDelete),
                                                new SqlParameter("ModifiedOn", modifiedOn)};
            return _unitOfWork.EnsureInTransaction(referralRepository.ExecuteNQStoredProc, "usp_DeleteReferralHeader", procParams);
        }
    }
}
