using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Security;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Referrals
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.Service.Registration.Referrals.IReferralSearchService" />
    public class ReferralSearchService : IReferralSearchService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;
        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ReferralSearch/";


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSearchService"/> class.
        /// </summary>
        public ReferralSearchService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSearchService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralSearchService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referrals.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ReferralSearchModel> GetReferrals(string searchStr, int searchType, long userID)
        {
            searchStr = string.IsNullOrWhiteSpace(searchStr) ? "" : searchStr;
            const string apiUrl = BaseRoute + "GetReferrals";
            var requestId = new NameValueCollection { { "searchStr", searchStr.ToString(CultureInfo.InvariantCulture) }, { "searchType", searchType.ToString(CultureInfo.InvariantCulture) }, { "userID", userID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<ReferralSearchModel>>(requestId, apiUrl);
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
            const string apiUrl = BaseRoute + "DeleteReferral";
            var requestId = new NameValueCollection { 
                                    { "id", id.ToString(CultureInfo.InvariantCulture) },
                                    { "reasonForDelete", reasonForDelete.ToString(CultureInfo.InvariantCulture) },
                                    { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return _communicationManager.Delete<Response<ReferralSearchModel>>(requestId, apiUrl);
        }
    }
}
