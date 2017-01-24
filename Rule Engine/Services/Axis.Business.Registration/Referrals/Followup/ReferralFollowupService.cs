using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Referrals.Followup
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralFollowupService : IReferralFollowupService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "referralFollowup/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ReferralFollowupService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralFollowupService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral followups.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> GetReferralFollowups(long referralHeaderID)
        {
            const string apiUrl = BaseRoute + "GetReferralFollowups";
            var requestId = new NameValueCollection { { "referralHeaderID", referralHeaderID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralOutcomeDetailsModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Gets the referral followup.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral outcome detail identifier.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> GetReferralFollowup(long referralOutcomeDetailID)
        {
            const string apiUrl = BaseRoute + "GetReferralFollowup";
            var requestId = new NameValueCollection { { "referralOutcomeDetailID", referralOutcomeDetailID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralOutcomeDetailsModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> AddReferralFollowup(ReferralOutcomeDetailsModel referral)
        {
            const string apiUrl = BaseRoute + "AddReferralFollowup";
            return communicationManager.Post<ReferralOutcomeDetailsModel, Response<ReferralOutcomeDetailsModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Updates the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> UpdateReferralFollowup(ReferralOutcomeDetailsModel referral)
        {
            const string apiUrl = BaseRoute + "UpdateReferralFollowup";
            return communicationManager.Put<ReferralOutcomeDetailsModel, Response<ReferralOutcomeDetailsModel>>(referral, apiUrl);
        }
    }
}