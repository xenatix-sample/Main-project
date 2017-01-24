using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Axis.DataProvider.CallCenter.CallCenterSummary
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.DataProvider.CallCenter.CallCenterSummary.ICallCenterSummaryDataProvider" />
    public class CallCenterSummaryDataProvider : ICallCenterSummaryDataProvider
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterSummaryDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CallCenterSummaryDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the call center summary.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <param name="userID">The user id.</param>
        /// <param name="searchTypeFilter">The search type filter.</param>
        /// <param name="callStatusID">The call status identifier.</param>
        /// <returns></returns>
        public Response<CallCenterSummaryModel> GetCallCenterSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID, int userIDFilter, bool isNormalView)
        {
            var sprocName = "usp_GetCallCenterSummarySearch";

            var procParams = new List<SqlParameter> {
                new SqlParameter("SearchCriteria", searchStr != null ? searchStr : string.Empty),
                new SqlParameter("UserID", userID),
                new SqlParameter("CallCenterTypeID", searchTypeFilter),
                new SqlParameter("CallStatusID", (object)callStatusID ?? DBNull.Value),
                new SqlParameter("SearchView", (object)isNormalView ?? DBNull.Value),
                new SqlParameter("UserIDFilter", (object)userIDFilter ?? DBNull.Value)
             };
            var callCenterSummaryRepository = _unitOfWork.GetRepository<CallCenterSummaryModel>(SchemaName.CallCenter);
            return callCenterSummaryRepository.ExecuteStoredProc(sprocName, procParams);
        }

        /// <summary>
        /// Gets the follow up summary.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterSummaryModel> GetFollowUpSummary(long callCenterHeaderID)
        {
            var sprocName = "usp_GetFollowupHistory";

            var procParams = new List<SqlParameter> {
                new SqlParameter("callCenterHeaderID", callCenterHeaderID),
                 new SqlParameter("UserID",  AuthContext.Auth.User.UserID)
             };
            var callCenterSummaryRepository = _unitOfWork.GetRepository<CallCenterSummaryModel>(SchemaName.CallCenter);
            return callCenterSummaryRepository.ExecuteStoredProc(sprocName, procParams);
        }

        /// <summary>
        /// Gets the law liaison incident.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<LawLiaisonIncidentModel> GetLawLiaisonIncident(long callCenterHeaderID)
        {
            var callCenterRepository = _unitOfWork.GetRepository<LawLiaisonIncidentModel>(SchemaName.CallCenter);

            var lawLiaisonIncident = callCenterRepository.ExecuteStoredProc("usp_GetIncidentHistoryLawLiaison", BuildCenterHeaderIDParams(callCenterHeaderID));
            var lawLiaisonIncidentRelatedItems = GetLawLiaisonIncidentRelatedItems(callCenterHeaderID);

            if (lawLiaisonIncident.DataItems == null || lawLiaisonIncident.DataItems.Count == 0)
            {
                lawLiaisonIncident = new Response<LawLiaisonIncidentModel>();
                lawLiaisonIncident.DataItems = new List<LawLiaisonIncidentModel> { new LawLiaisonIncidentModel() };
            }
            lawLiaisonIncident.DataItems.FirstOrDefault().RelatedItems = lawLiaisonIncidentRelatedItems.DataItems;

            return lawLiaisonIncident;
        }

        /// <summary>
        /// Gets the law liaison incident related items.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        private Response<LawLiaisonIncidentRelatedItemsModel> GetLawLiaisonIncidentRelatedItems(long callCenterHeaderID)
        {
            var callCenterSummaryRepository = _unitOfWork.GetRepository<LawLiaisonIncidentRelatedItemsModel>(SchemaName.CallCenter);
            return callCenterSummaryRepository.ExecuteStoredProc("usp_GetRelatedItemsHistoryLawLiaison", BuildCenterHeaderIDParams(callCenterHeaderID));
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<CallCenterSummaryModel> Delete(long id, DateTime modifiedOn)
        {
            var callCenterSummaryRepository = _unitOfWork.GetRepository<CallCenterSummaryModel>(SchemaName.CallCenter);
            var procParams = new List<SqlParameter> { new SqlParameter("CallCenterHeaderID", id), new SqlParameter("ModifiedOn", modifiedOn) };
            return _unitOfWork.EnsureInTransaction(callCenterSummaryRepository.ExecuteNQStoredProc, "usp_DeleteCallCenter", procParams);
        }

        /// <summary>
        /// Builds the center header identifier parameters.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildCenterHeaderIDParams(long callCenterHeaderID)
        {
            return new List<SqlParameter> {
                new SqlParameter("callCenterHeaderID", callCenterHeaderID),
                new SqlParameter("UserID",  AuthContext.Auth.User.UserID)
             };
        }
    }
}
