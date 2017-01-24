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
    public class ReferralPhoneRepository : IReferralPhoneRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "referralPhone/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralPhoneRepository" /> class.
        /// </summary>
        public ReferralPhoneRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralPhoneRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralPhoneRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the Phonees.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralPhoneViewModel> GetPhones(long referralID, int contactTypeID)
        {
            const string apiUrl = baseRoute + "GetPhones";
            var param = new NameValueCollection { { "referralID", referralID.ToString(CultureInfo.InvariantCulture) },
                                        { "contactTypeID", contactTypeID.ToString(CultureInfo.InvariantCulture) }};
            return communicationManager.Get<Response<ReferralPhoneModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds and update Phonees.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralPhoneViewModel> AddUpdatePhones(List<ReferralPhoneViewModel> referral)
        {
            const string apiUrl = baseRoute + "AddUpdatePhones";
            var response = communicationManager.Post<List<ReferralPhoneModel>, Response<ReferralPhoneModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the Phone.
        /// </summary>
        /// <param name="referralPhoneID">The referral Phone identifier.</param>
        /// <returns></returns>
        public Response<ReferralPhoneViewModel> DeleteReferralPhone(long referralPhoneID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteReferralPhone";
            var param = new NameValueCollection { { "referralPhoneID", referralPhoneID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralPhoneModel>>(param, apiUrl).ToViewModel();
        }
    }
}