using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Referrals.Information
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralReferredInformationService : IReferralReferredInformationService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ReferralReferredInformation/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ReferralReferredInformationService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralReferredInformationService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral referred information
        /// </summary>
        /// <param name="referralHeaderID">The referralHeader id.</param>
        /// <returns></returns>
        public Response<ReferralReferredInformationModel> GetReferredInformation(long referralHeaderID)
        {
            const string apiUrl = BaseRoute + "GetReferredInformation";
            var requestId = new NameValueCollection { { "referralHeaderID", referralHeaderID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralReferredInformationModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the referral referred information
        /// </summary>
        /// <param name="Referral">The referral referred information.</param>
        /// <returns></returns>
        public Response<ReferralReferredInformationModel> AddReferredInformation(ReferralReferredInformationModel referral)
        {
            const string apiUrl = BaseRoute + "AddReferredInformation";
            return communicationManager.Post<ReferralReferredInformationModel, Response<ReferralReferredInformationModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Updates the referral referred information.
        /// </summary>
        /// <param name="referral">The referred information.</param>
        /// <returns></returns>
        public Response<ReferralReferredInformationModel> UpdateReferredInformation(ReferralReferredInformationModel referral)
        {
            const string apiUrl = BaseRoute + "UpdateReferredInformation";
            return communicationManager.Put<ReferralReferredInformationModel, Response<ReferralReferredInformationModel>>(referral, apiUrl);
        }



       
    }
}