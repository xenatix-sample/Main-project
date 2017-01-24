using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Translator.Referrals;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Registration.Repository.Referrals.Requestor
{
    public class ReferralDemographicsRepository : IReferralDemographicsRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "referralDemographics/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDemographicsRepository" /> class.
        /// </summary>
        public ReferralDemographicsRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDemographicsRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralDemographicsRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsViewModel> GetReferralDemographics(long referralID)
        {
            const string apiUrl = baseRoute + "GetReferralDemographics";
            var param = new NameValueCollection { { "referralID", referralID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralDemographicsModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the referral demographics.
        /// </summary>
        /// <param name="referralDemographics">The referral demographics.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsViewModel> AddReferralDemographics(ReferralDemographicsViewModel referralDemographics)
        {
            const string apiUrl = baseRoute + "AddReferralDemographics";
            var response = communicationManager.Post<ReferralDemographicsModel, Response<ReferralDemographicsModel>>(referralDemographics.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the referral demographics.
        /// </summary>
        /// <param name="referralDemographics">The referral demographics.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsViewModel> UpdateReferralDemographics(ReferralDemographicsViewModel referralDemographics)
        {
            const string apiUrl = baseRoute + "UpdateReferralDemographics";
            var response = communicationManager.Put<ReferralDemographicsModel, Response<ReferralDemographicsModel>>(referralDemographics.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsViewModel> DeleteReferralDemographics(long referralID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteReferralDemographics";
            var param = new NameValueCollection { { "referralID", referralID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralDemographicsModel>>(param, apiUrl).ToViewModel();
        }
    }
}