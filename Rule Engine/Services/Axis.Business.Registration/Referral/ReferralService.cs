using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;

namespace Axis.Service.Registration
{
    public class ReferralService : IReferralService
    {
        private readonly CommunicationManager communicationManager;
        private const string BaseRoute = "referral/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ReferralService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public ReferralService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets referrals
        /// </summary>
        /// <param name="contactId">Contact Id</param>
        /// <returns></returns>
        public Response<ReferralModel> GetReferrals(long contactId)
        {
            const string apiUrl = BaseRoute + "GetReferrals";
            var requestId = new NameValueCollection {{"contactId", contactId.ToString(CultureInfo.InvariantCulture)}};
            return communicationManager.Get<Response<ReferralModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds referral
        /// </summary>
        /// <param name="referral">Referral model</param>
        /// <returns></returns>
        public Response<ReferralModel> AddReferral(ReferralModel referral)
        {
            const string apiUrl = BaseRoute + "AddReferral";
            return communicationManager.Post<ReferralModel, Response<ReferralModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Updates referral
        /// </summary>
        /// <param name="referral"></param>
        /// <returns></returns>
        public Response<ReferralModel> UpdateReferral(ReferralModel referral)
        {
            const string apiUrl = BaseRoute + "UpdateReferral";
            return communicationManager.Put<ReferralModel, Response<ReferralModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Deletes referral
        /// </summary>
        /// <param name="id">Referral Id</param>
        /// <returns></returns>
        public Response<ReferralModel> DeleteReferral(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteReferral";
            var requestId = new NameValueCollection
            {
                { "ReferralId", id.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ReferralModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Update referral contact
        /// </summary>
        /// <param name="referralContact"></param>
        /// <returns></returns>
        public Response<ReferralContactModel> UpdateReferralContact(ReferralContactModel referralContact)
        {
            const string apiUrl = BaseRoute + "UpdateReferralContact";
            return communicationManager.Put<ReferralContactModel, Response<ReferralContactModel>>(referralContact, apiUrl);
        }

        /// <summary>
        /// Delete referal contact
        /// </summary>
        /// <param name="referralContactId"></param>
        /// <returns></returns>
        public Response<ReferralContactModel> DeleteReferalContact(long referralContactId, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteReferralContact";
            var requestId = new NameValueCollection
            {
                {"referralContactId", referralContactId.ToString(CultureInfo.InvariantCulture)},
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ReferralContactModel>>(requestId, apiUrl);
        }
    }
}
