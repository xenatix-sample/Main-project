using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Referral
{
    public class ReferralConcernDetailService : IReferralConcernDetailService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ReferralConcernDetail/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralConcernDetailService"/> class.
        /// </summary>
        public ReferralConcernDetailService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralConcernDetailService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralConcernDetailService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> GetReferralConcernDetail(long referralAdditionalDetailID)
        {
            const string apiUrl = BaseRoute + "GetReferralConcernDetail";
            var requestId = new NameValueCollection { { "referralAdditionalDetailID", referralAdditionalDetailID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralConcernDetailModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the referral ConcernDetail.
        /// </summary>
        /// <param name="ReferralConcernDetail">The referral ConcernDetail.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> AddReferralConcernDetail(ReferralConcernDetailModel ReferralConcernDetail)
        {
            const string apiUrl = BaseRoute + "AddReferralConcernDetail";
            return communicationManager.Post<ReferralConcernDetailModel, Response<ReferralConcernDetailModel>>(ReferralConcernDetail, apiUrl);
        }

        
        public Response<ReferralConcernDetailModel> UpdateReferralConcernDetail(ReferralConcernDetailModel ReferralConcernDetail)
        {
            const string apiUrl = BaseRoute + "UpdateReferralConcernDetail";
            return communicationManager.Put<ReferralConcernDetailModel, Response<ReferralConcernDetailModel>>(ReferralConcernDetail, apiUrl);
        }

        /// <summary>
        /// Gets the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> DeleteReferralConcernDetail(long referralConcernDetailID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteReferralConcernDetail";
            var requestId = new NameValueCollection
            {
                { "referralConcernDetailID", referralConcernDetailID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };

            return communicationManager.Delete<Response<ReferralConcernDetailModel>>(requestId, apiUrl);
        }
    }
}