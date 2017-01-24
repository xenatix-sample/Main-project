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
    public class ReferralEmailRepository : IReferralEmailRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ReferralEmail/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralEmailRepository" /> class.
        /// </summary>
        public ReferralEmailRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralEmailRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralEmailRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the Emails.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralEmailViewModel> GetEmails(long referralID, int contactTypeID)
        {
            const string apiUrl = baseRoute + "GetEmails";
            var param = new NameValueCollection { { "referralID", referralID.ToString(CultureInfo.InvariantCulture) },
                                        { "contactTypeID", contactTypeID.ToString(CultureInfo.InvariantCulture) }};
            return communicationManager.Get<Response<ReferralEmailModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds and update Emails.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralEmailViewModel> AddUpdateEmails(List<ReferralEmailViewModel> referral)
        {
            const string apiUrl = baseRoute + "AddUpdateEmails";
            var response = communicationManager.Post<List<ReferralEmailModel>, Response<ReferralEmailModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the Email.
        /// </summary>
        /// <param name="referralEmailID">The referral Email identifier.</param>
        /// <returns></returns>
        public Response<ReferralEmailViewModel> DeleteEmail(long referralEmailID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteEmail";
            var param = new NameValueCollection { { "referralEmailID", referralEmailID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReferralEmailModel>>(param, apiUrl).ToViewModel();
        }
    }
}