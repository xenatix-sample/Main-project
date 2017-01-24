using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Translator.Referrals;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Registration.Repository.Referrals.Followup
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralFollowupRepository : IReferralFollowupRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "referralFollowup/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralFollowupRepository"/> class.
        /// </summary>
        public ReferralFollowupRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralFollowupRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralFollowupRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral followups.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        
        public Response<ReferralOutcomeDetailsViewModel> GetReferralFollowups(long referralHeaderID)
        {
            const string apiUrl = baseRoute + "GetReferralFollowups";
            var param = new NameValueCollection { { "referralHeaderID", referralHeaderID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralOutcomeDetailsModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Gets the referral followup.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral outcome detail identifier.</param>
        /// <returns></returns>
    
        public Response<ReferralOutcomeDetailsViewModel> GetReferralFollowup(long referralOutcomeDetailID)
        {
            const string apiUrl = baseRoute + "GetReferralFollowup";
            var param = new NameValueCollection { { "referralOutcomeDetailID", referralOutcomeDetailID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralOutcomeDetailsModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
       
        public Response<ReferralOutcomeDetailsViewModel> AddReferralFollowup(ReferralOutcomeDetailsViewModel referral)
        {
            const string apiUrl = baseRoute + "AddReferralFollowup";
            var response = communicationManager.Post<ReferralOutcomeDetailsModel, Response<ReferralOutcomeDetailsModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
    
        public Response<ReferralOutcomeDetailsViewModel> UpdateReferralFollowup(ReferralOutcomeDetailsViewModel referral)
        {
            const string apiUrl = baseRoute + "UpdateReferralFollowup";
            var response = communicationManager.Put<ReferralOutcomeDetailsModel, Response<ReferralOutcomeDetailsModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }
    }
}