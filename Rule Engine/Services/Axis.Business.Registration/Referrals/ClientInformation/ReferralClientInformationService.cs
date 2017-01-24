using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Referrals.ClientInformation
{
    public class ReferralClientInformationService : IReferralClientInformationService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ReferralClientInformation/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ReferralClientInformationService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralClientInformationService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the Referral Client information
        /// </summary>
        /// <param name="ReferredToInformationID">The Detail identifier.</param>
        /// <returns></returns>
        public Response<ReferralClientInformationModel> GetClientInformation(long ReferralID)
        {
            const string apiUrl = BaseRoute + "GetClientInformation";
            var requestId = new NameValueCollection { { "referralHeaderID", ReferralID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralClientInformationModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the referral referred information
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        public Response<ReferralClientInformationModel> AddClientInformation(ReferralClientInformationModel clientInformation)
        {
            const string apiUrl = BaseRoute + "AddClientInformation";
            return communicationManager.Post<ReferralClientInformationModel, Response<ReferralClientInformationModel>>(clientInformation, apiUrl);
        }

        /// <summary>
        /// Updates the referral referred information 
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        public Response<ReferralClientInformationModel> UpdateClientInformation(ReferralClientInformationModel clientInformation)
        {
            const string apiUrl = BaseRoute + "UpdateClientInformation";
            return communicationManager.Put<ReferralClientInformationModel, Response<ReferralClientInformationModel>>(clientInformation, apiUrl);
        }
    }
}
