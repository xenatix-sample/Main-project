using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Referrals.Forwarded
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralForwardedService : IReferralForwardedService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "referralForwarded/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ReferralForwardedService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralForwardedService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral forwarded.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> GetReferralForwardedDetails(long ReferralHeaderID)
        {
            const string apiUrl = BaseRoute + "GetReferralForwardedDetails";
            var requestId = new NameValueCollection { { "ReferralHeaderID", ReferralHeaderID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralForwardedModel>>(requestId, apiUrl);
        }
        /// <summary>
        /// Gets the referral Forwarded.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral outcome detail identifier.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> GetReferralForwardedDetail(long ReferralForwardedDetailID)
        {
            const string apiUrl = BaseRoute + "GetReferralForwardedDetail";
            var requestId = new NameValueCollection { { "ReferralForwardedDetailID", ReferralForwardedDetailID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralForwardedModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the referral Forwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> AddReferralForwardedDetail(ReferralForwardedModel referral)
        {
            const string apiUrl = BaseRoute + "AddReferralForwardedDetail";
            return communicationManager.Post<ReferralForwardedModel, Response<ReferralForwardedModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Updates the referral Forwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> UpdateReferralForwardedDetail(ReferralForwardedModel referral)
        {
            const string apiUrl = BaseRoute + "UpdateReferralForwardedDetail";
            return communicationManager.Put<ReferralForwardedModel, Response<ReferralForwardedModel>>(referral, apiUrl);
        }

        
    }
}