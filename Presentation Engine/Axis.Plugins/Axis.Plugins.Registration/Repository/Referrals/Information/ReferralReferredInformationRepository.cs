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

namespace Axis.Plugins.Registration.Repository.Referrals.Information
{
    /// <summary>
    /// Referral referred to information repository
    /// </summary>
    public class ReferralReferredInformationRepository : IReferralReferredInformationRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "referralReferredInformation/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralReferredInformationRepository"/> class.
        /// </summary>
        public ReferralReferredInformationRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralFollowupRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralReferredInformationRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral referred information
        /// </summary>
        /// <param name="referralHeaderID">The referralHeader id.</param>
        /// <returns></returns>

        
        public Response<ReferralReferredInformationViewModel> GetReferredInformation(long referralHeaderID)
        {
            const string apiUrl = BaseRoute + "GetReferredInformation";
            var param = new NameValueCollection { { "referralHeaderID", referralHeaderID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralReferredInformationModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the referral referred information
        /// </summary>
        /// <param name="Referral">The referral referred information.</param>
        /// <returns></returns>
    
        public Response<ReferralReferredInformationViewModel> AddReferredInformation(ReferralReferredInformationViewModel referral)
        {
            const string apiUrl = BaseRoute + "AddReferredInformation";
            var response = communicationManager.Post<ReferralReferredInformationModel, Response<ReferralReferredInformationModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
            
        }

        /// <summary>
        /// Updates the referral referred information.
        /// </summary>
        /// <param name="referral">The referred information.</param>
        /// <returns></returns>
     
        public Response<ReferralReferredInformationViewModel> UpdateReferredInformation(ReferralReferredInformationViewModel referral)
        {
            const string apiUrl = BaseRoute + "UpdateReferredInformation";
            return communicationManager.Put<ReferralReferredInformationModel, Response<ReferralReferredInformationViewModel>>(referral.ToModel(), apiUrl);
        }       
    }
}