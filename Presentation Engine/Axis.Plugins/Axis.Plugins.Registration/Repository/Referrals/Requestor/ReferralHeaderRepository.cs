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
    public class ReferralHeaderRepository : IReferralHeaderRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ReferralHeader/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralHeaderRepository" /> class.
        /// </summary>
        public ReferralHeaderRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralHeaderRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralHeaderRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral header.
        /// </summary>
        /// <param name="referralHeaderID">The referral header identifier.</param>
        /// <returns></returns>
        public Response<ReferralHeaderViewModel> GetReferralHeader(long referralHeaderID)
        {
            const string apiUrl = baseRoute + "GetReferralHeader";
            var param = new NameValueCollection { { "referralHeaderID", referralHeaderID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralHeaderModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the referral header.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        /// <returns></returns>
        public Response<ReferralHeaderViewModel> AddReferralHeader(ReferralHeaderViewModel referralHeader)
        {
            const string apiUrl = baseRoute + "AddReferralHeader";
            var response = communicationManager.Post<ReferralHeaderModel, Response<ReferralHeaderModel>>(referralHeader.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the referral header.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        /// <returns></returns>
        public Response<ReferralHeaderViewModel> UpdateReferralHeader(ReferralHeaderViewModel referralHeader)
        {
            const string apiUrl = baseRoute + "UpdateReferralHeader";
            var response = communicationManager.Put<ReferralHeaderModel, Response<ReferralHeaderModel>>(referralHeader.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the referral header.
        /// </summary>
        /// <param name="referralHeaderID">The referral header identifier.</param>
        /// <returns></returns>
        public Response<ReferralHeaderViewModel> DeleteReferralHeader(long referralHeaderID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteReferralHeader";
            var param = new NameValueCollection { { "referralHeaderID", referralHeaderID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralHeaderModel>>(param, apiUrl).ToViewModel();
        }
    }
}