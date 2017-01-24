using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using Axis.Plugins.Registration.Models.Referrals.Common;
using Axis.Plugins.Registration.Translator.Referrals.Common;
using Axis.Service;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Registration.Repository.Referrals.Common
{
    public class ReferralAddressRepository : IReferralAddressRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "referralAddress/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralAddressRepository" /> class.
        /// </summary>
        public ReferralAddressRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralAddressRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralAddressRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralAddressViewModel> GetAddresses(long referralID, int contactTypeID)
        {
            const string apiUrl = baseRoute + "GetAddresses";
            var param = new NameValueCollection { { "referralID", referralID.ToString(CultureInfo.InvariantCulture) },
                                        { "contactTypeID", contactTypeID.ToString(CultureInfo.InvariantCulture) }};
            return communicationManager.Get<Response<ReferralAddressModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds and update addresses.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralAddressViewModel> AddUpdateAddresses(List<ReferralAddressViewModel> referral)
        {
            const string apiUrl = baseRoute + "AddUpdateAddresses";
            var response = communicationManager.Post<List<ReferralAddressModel>, Response<ReferralAddressModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="referralAddressID">The referral address identifier.</param>
        /// <returns></returns>
        public Response<ReferralAddressViewModel> DeleteAddress(long referralAddressID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteAddress";
            var param = new NameValueCollection { { "referralAddressID", referralAddressID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralAddressModel>>(param, apiUrl).ToViewModel();
        }
    }
}