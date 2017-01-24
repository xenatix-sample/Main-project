using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Referrals;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Referral
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.Service.Registration.Referral.IReferralAdditionalDetailService" />
    public class ReferralAdditionalDetailService : IReferralAdditionalDetailService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ReferralAdditionalDetail/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralService" /> class.
        /// </summary>
        public ReferralAdditionalDetailService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralService" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralAdditionalDetailService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailModel> GetReferralAdditionalDetail(long contactID)
        {
            const string apiUrl = BaseRoute + "GetReferralAdditionalDetail";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralAdditionalDetailModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReferralDetailsModel> GetReferralsDetails(long contactID)
        {
            const string apiUrl = BaseRoute + "GetReferralsDetails";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralDetailsModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the referral .
        /// </summary>
        /// <param name="referral">The referral .</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailModel> AddReferralAdditionalDetail(ReferralAdditionalDetailModel referral)
        {
            const string apiUrl = BaseRoute + "AddReferralAdditionalDetail";
            return communicationManager.Post<ReferralAdditionalDetailModel, Response<ReferralAdditionalDetailModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Updates the referral .
        /// </summary>
        /// <param name="referral">The referral .</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailModel> UpdateReferralAdditionalDetail(ReferralAdditionalDetailModel referral)
        {
            const string apiUrl = BaseRoute + "UpdateReferralAdditionalDetail";
            return communicationManager.Put<ReferralAdditionalDetailModel, Response<ReferralAdditionalDetailModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Deletes the referral details.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReferralDetailsModel> DeleteReferralDetails(long contactID)
        {
            const string apiUrl = BaseRoute + "DeleteReferralDetails";
            var requestId = new NameValueCollection
            {
                { "contactID", contactID.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ReferralDetailsModel>>(requestId, apiUrl);
        }
    }
}
