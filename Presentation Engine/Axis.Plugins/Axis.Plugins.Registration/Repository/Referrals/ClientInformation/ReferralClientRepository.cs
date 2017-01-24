using Axis.Configuration;
using Axis.Helpers;
using Axis.Service;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Translator.Referrals;
using Axis.Model.Common;
using Axis.Constant;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Model.Registration.Referrals;


namespace Axis.Plugins.Registration.Repository.Referrals.ClientInformation
{
    public class ReferralClientInformationRepository : IReferralClientInformationRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ReferralClientInformation/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralClientInformationRepository"/> class.
        /// </summary>
        public ReferralClientInformationRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralClientInformationRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralClientInformationRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the Referral Client information
        /// </summary>
        /// <param name="referralHeaderID">The referral header identifier.</param>
        /// <returns></returns>
      
        public Response<ReferralClientInformationModel> GetClientInformation(long referralHeaderID)
        {
            const string apiUrl = baseRoute + "GetClientInformation";
            var param = new NameValueCollection { { "ReferralID", referralHeaderID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralClientInformationModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds the referral client.
        /// </summary>
        /// <param name="referral">The referral client.</param>
        /// <returns></returns>
     
        public Response<ReferralClientInformationModel> AddClientInformation(ReferralClientInformationModel referral)
        {
            const string apiUrl = baseRoute + "AddClientInformation";
            return communicationManager.Post<ReferralClientInformationModel, Response<ReferralClientInformationModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Updates the referral client.
        /// </summary>
        /// <param name="referral">The referral client.</param>
        /// <returns></returns>
       
        public Response<ReferralClientInformationModel> UpdateClientInformation(ReferralClientInformationModel referral)
        {
            const string apiUrl = baseRoute + "UpdateClientInformation";
            return communicationManager.Put<ReferralClientInformationModel, Response<ReferralClientInformationModel>>(referral, apiUrl);
        }
    }
}
