using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Referrals.Requestor
{
    public class ReferralDemographicsService : IReferralDemographicsService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ReferralDemographics/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDemographicsService"/> class.
        /// </summary>
        public ReferralDemographicsService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDemographicsService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralDemographicsService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> GetReferralDemographics(long referralID)
        {
            const string apiUrl = BaseRoute + "GetReferralDemographics";
            var requestId = new NameValueCollection { { "referralID", referralID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralDemographicsModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the referral demographics.
        /// </summary>
        /// <param name="referralDemographics">The referral demographics.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> AddReferralDemographics(ReferralDemographicsModel referralDemographics)
        {
            const string apiUrl = BaseRoute + "AddReferralDemographics";
            return communicationManager.Post<ReferralDemographicsModel, Response<ReferralDemographicsModel>>(referralDemographics, apiUrl);
        }

        /// <summary>
        /// Updates the referral demographics.
        /// </summary>
        /// <param name="referralDemographics">The referral demographics.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> UpdateReferralDemographics(ReferralDemographicsModel referralDemographics)
        {
            const string apiUrl = BaseRoute + "UpdateReferralDemographics";
            return communicationManager.Put<ReferralDemographicsModel, Response<ReferralDemographicsModel>>(referralDemographics, apiUrl);
        }

        /// <summary>
        /// Deletes the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> DeleteReferralDemographics(long referralID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteReferralDemographics";
            var requestId = new NameValueCollection
            {
                { "referralID", referralID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };

            return communicationManager.Get<Response<ReferralDemographicsModel>>(requestId, apiUrl);
        }
    }
}