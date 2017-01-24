using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Translator.Referrals;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Constant;

namespace Axis.Plugins.Registration.Repository.Referrals.Disposition
{
    public class ReferralDispositionRepository : IReferralDispositionRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "referralDisposition/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDispositionRepository"/> class.
        /// </summary>
        public ReferralDispositionRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDispositionRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralDispositionRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral disposition detail.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral disposition detail identifier.</param>
        /// <returns></returns>
      
        public Response<ReferralDispositionViewModel> GetReferralDispositionDetail(long referralHeaderID)
        {
            const string apiUrl = baseRoute + "GetReferralDispositionDetail";
            var param = new NameValueCollection { { "referralHeaderID", referralHeaderID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralDispositionModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
       
        public Response<ReferralDispositionViewModel> AddReferralDisposition(ReferralDispositionViewModel referral)
        {
            const string apiUrl = baseRoute + "AddReferralDisposition";
            var response = communicationManager.Post<ReferralDispositionModel, Response<ReferralDispositionModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
      
        public Response<ReferralDispositionViewModel> UpdateReferralDisposition(ReferralDispositionViewModel referral)
        {
            const string apiUrl = baseRoute + "UpdateReferralDisposition";
            var response = communicationManager.Put<ReferralDispositionModel, Response<ReferralDispositionModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }
    }
}
