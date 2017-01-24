using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Translator.Referrals;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Registration.Repository.Referrals.Forwarded
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralForwardedRepository : IReferralForwardedRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "referralForwarded/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralForwardedRepository"/> class.
        /// </summary>
        public ReferralForwardedRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralForwardedRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralForwardedRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral forwarded
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
      
        public Response<ReferralForwardedViewModel> GetReferralForwardedDetails(long referralHeaderID)
        {
            const string apiUrl = baseRoute + "GetReferralForwardedDetails";
            var param = new NameValueCollection { { "referralHeaderID", referralHeaderID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralForwardedModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Gets the referral Forwarded.
        /// </summary>
        /// <param name="referralForwardedDetailID">The referral Forwarded detail identifier.</param>
        /// <returns></returns>
       
        public Response<ReferralForwardedViewModel> GetReferralForwardedDetail(long referralForwardedDetailID)
        {
            const string apiUrl = baseRoute + "GetReferralForwardedDetail";
            var param = new NameValueCollection { { "ReferralForwardedDetailID", referralForwardedDetailID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralForwardedModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the referral Forwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
      
        public Response<ReferralForwardedViewModel> AddReferralForwardedDetail(ReferralForwardedViewModel referral)
        {
            const string apiUrl = baseRoute + "AddReferralForwardedDetail";
            var response = communicationManager.Post<ReferralForwardedModel, Response<ReferralForwardedModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the referral Forwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
      
        public Response<ReferralForwardedViewModel> UpdateReferralForwardedDetail(ReferralForwardedViewModel referral)
        {
            const string apiUrl = baseRoute + "UpdateReferralForwardedDetail";
            var response = communicationManager.Put<ReferralForwardedModel, Response<ReferralForwardedModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

       
    }
}