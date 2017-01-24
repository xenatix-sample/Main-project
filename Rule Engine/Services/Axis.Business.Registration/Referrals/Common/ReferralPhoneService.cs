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
    public class ReferralPhoneService : IReferralPhoneService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ReferralPhones/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralPhoneService"/> class.
        /// </summary>
        public ReferralPhoneService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralPhoneService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralPhoneService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the phones.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralPhoneModel> GetPhones(long referralID, int contactTypeID)
        {
            const string apiUrl = BaseRoute + "GetPhones";
            var requestId = new NameValueCollection { { "referralID", referralID.ToString(CultureInfo.InvariantCulture) },
                            { "contactTypeID", contactTypeID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralPhoneModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Add and update phones.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralPhoneModel> AddUpdatePhones(List<ReferralPhoneModel> referral)
        {
            const string apiUrl = BaseRoute + "AddUpdatePhones";
            return communicationManager.Post<List<ReferralPhoneModel>, Response<ReferralPhoneModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Deletes the referral phone.
        /// </summary>
        /// <param name="referralPhoneID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralPhoneModel> DeleteReferralPhone(long referralPhoneID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteReferralPhone";
            var requestId = new NameValueCollection
            {
                { "referralID", referralPhoneID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };

            return communicationManager.Get<Response<ReferralPhoneModel>>(requestId, apiUrl);
        }
    }
}