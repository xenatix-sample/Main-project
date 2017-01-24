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
    public class ReferralAddressService : IReferralAddressService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ReferralAddress/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralAddressService"/> class.
        /// </summary>
        public ReferralAddressService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralAddressService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralAddressService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralAddressModel> GetAddresses(long referralID, int contactTypeID)
        {
            const string apiUrl = BaseRoute + "GetAddresses";
            var requestId = new NameValueCollection { { "referralID", referralID.ToString(CultureInfo.InvariantCulture) },
                            { "contactTypeId", contactTypeID.ToString(CultureInfo.InvariantCulture) } };

            return communicationManager.Get<Response<ReferralAddressModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds and update addresses.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralAddressModel> AddUpdateAddresses(List<ReferralAddressModel> referral)
        {
            const string apiUrl = BaseRoute + "AddUpdateAddresses";
            return communicationManager.Post<List<ReferralAddressModel>, Response<ReferralAddressModel>>(referral, apiUrl);
        }

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="referralAddressID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralAddressModel> DeleteAddress(long referralAddressID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteAddress";
            var requestId = new NameValueCollection
            {
                { "referralID", referralAddressID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };

            return communicationManager.Get<Response<ReferralAddressModel>>(requestId, apiUrl);
        }
    }
}