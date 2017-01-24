using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Registration.Repository.Referral
{
    public class ReferralConcernDetailRepository : IReferralConcernDetailRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ReferralConcernDetail/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralConcernDetailRepository" /> class.
        /// </summary>
        public ReferralConcernDetailRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralConcernDetailRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralConcernDetailRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailViewModel> GetReferralConcernDetail(long referralAdditionalDetailID)
        {
            const string apiUrl = baseRoute + "GetReferralConcernDetail";
            var param = new NameValueCollection { { "referralAdditionalDetailID", referralAdditionalDetailID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralConcernDetailModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the referral ConcernDetail.
        /// </summary>
        /// <param name="ReferralConcernDetail">The referral ConcernDetail.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailViewModel> AddReferralConcernDetail(ReferralConcernDetailViewModel referralConcernDetail)
        {
            const string apiUrl = baseRoute + "AddReferralConcernDetail";
            var response = communicationManager.Post<ReferralConcernDetailModel, Response<ReferralConcernDetailModel>>(referralConcernDetail.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the referral ConcernDetail.
        /// </summary>
        /// <param name="ReferralConcernDetail">The referral ConcernDetail.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailViewModel> UpdateReferralConcernDetail(ReferralConcernDetailViewModel referralConcernDetail)
        {
            const string apiUrl = baseRoute + "UpdateReferralConcernDetail";
            var response = communicationManager.Put<ReferralConcernDetailModel, Response<ReferralConcernDetailModel>>(referralConcernDetail.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailViewModel> DeleteReferralConcernDetail(long referralConcernDetailID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteReferralConcernDetail";
            var param = new NameValueCollection { { "referralConcernDetailID", referralConcernDetailID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Delete<Response<ReferralConcernDetailModel>>(param, apiUrl).ToViewModel();
        }
    }
}