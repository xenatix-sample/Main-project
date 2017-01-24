using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using Axis.Security;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Referrals.Common
{
    public class ReferralEmailService : IReferralEmailService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ReferralEmail/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralEmailService"/> class.
        /// </summary>
        public ReferralEmailService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralEmailService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralEmailService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralEmailModel> GetEmails(long referralID, int contactTypeID)
        {
            const string apiUrl = BaseRoute + "GetEmails";
            var requestId = new NameValueCollection { { "referralID", referralID.ToString(CultureInfo.InvariantCulture) },
                            { "contactTypeID", contactTypeID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralEmailModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds and update emails.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralEmailModel> AddUpdateEmails(List<ReferralEmailModel> referral)
        {
            const string apiUrl = BaseRoute + "AddUpdateEmails";
            return communicationManager.Post<List<ReferralEmailModel>, Response<ReferralEmailModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Deletes the email.
        /// </summary>
        /// <param name="referralEmailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralEmailModel> DeleteEmail(long referralEmailID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteEmail";
            var requestId = new NameValueCollection
            {
                { "referralID", referralEmailID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };

            return communicationManager.Get<Response<ReferralEmailModel>>(requestId, apiUrl);
        }
    }
}