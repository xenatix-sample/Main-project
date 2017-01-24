using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration.Referrals.Disposition
{
    public class ReferralDispositionService : IReferralDispositionService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "referralDisposition/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ReferralDispositionService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralDispositionService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral disposition detail.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral disposition detail identifier.</param>
        /// <returns></returns>
        public Response<ReferralDispositionModel> GetReferralDispositionDetail(long referralHeaderID)
        {
            const string apiUrl = BaseRoute + "GetReferralDispositionDetail";
            var requestId = new NameValueCollection { { "referralHeaderID", referralHeaderID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralDispositionModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the referral followup disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        public Response<ReferralDispositionModel> AddReferralDisposition(ReferralDispositionModel referralDisposition)
        {
            const string apiUrl = BaseRoute + "AddReferralDisposition";
            return communicationManager.Post<ReferralDispositionModel, Response<ReferralDispositionModel>>(referralDisposition, apiUrl);
        }

        /// <summary>
        /// Updates the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        public Response<ReferralDispositionModel> UpdateReferralDisposition(ReferralDispositionModel referralDisposition)
        {
            const string apiUrl = BaseRoute + "UpdateReferralDisposition";
            return communicationManager.Put<ReferralDispositionModel, Response<ReferralDispositionModel>>(referralDisposition, apiUrl);
        }
    }
}
