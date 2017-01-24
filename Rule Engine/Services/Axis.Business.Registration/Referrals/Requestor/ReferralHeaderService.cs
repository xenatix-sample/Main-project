using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Referrals.Requestor
{
    public class ReferralHeaderService : IReferralHeaderService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ReferralHeader/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralHeaderService"/> class.
        /// </summary>
        public ReferralHeaderService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralHeaderService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralHeaderService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral header.
        /// </summary>
        /// <param name="referralHeaderID">The referral header identifier.</param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> GetReferralHeader(long referralHeaderID)
        {
            const string apiUrl = BaseRoute + "GetReferralHeader";
            var requestId = new NameValueCollection { { "referralHeaderID", referralHeaderID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralHeaderModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the referral header.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> AddReferralHeader(ReferralHeaderModel referralHeader)
        {
            const string apiUrl = BaseRoute + "AddReferralHeader";
            return communicationManager.Post<ReferralHeaderModel, Response<ReferralHeaderModel>>(referralHeader, apiUrl);
        }

        /// <summary>
        /// Updates the referral header.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> UpdateReferralHeader(ReferralHeaderModel referralHeader)
        {
            const string apiUrl = BaseRoute + "UpdateReferralHeader";
            return communicationManager.Put<ReferralHeaderModel, Response<ReferralHeaderModel>>(referralHeader, apiUrl);
        }

        /// <summary>
        /// Deletes the referral header.
        /// </summary>
        /// <param name="referralHeaderID">The referral header identifier.</param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> DeleteReferralHeader(long referralHeaderID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteReferralHeader";
            var requestId = new NameValueCollection
            {
                { "referralID", referralHeaderID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };

            return communicationManager.Get<Response<ReferralHeaderModel>>(requestId, apiUrl);
        }
    }
}