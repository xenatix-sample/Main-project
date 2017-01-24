using System;
using System.Threading.Tasks;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Translator;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    /// Respository for referral
    /// </summary>
    public class ReferralRepository : IReferralRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "referral/";

        public ReferralRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public ReferralRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Get referrals
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public Response<ReferralModel> GetReferrals(long contactId)
        {
            return GetReferralsAsync(contactId).Result;
        }

        /// <summary>
        /// Get referrals asynchronously
        /// </summary>
        /// <param name="contactId">Contact Id</param>
        /// <returns></returns>
        public async Task<Response<ReferralModel>> GetReferralsAsync(long contactId)
        {
            const string apiUrl = baseRoute + "GetReferrals";
            var param = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return await communicationManager.GetAsync<Response<ReferralModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds referral
        /// </summary>
        /// <param name="referral">referral model</param>
        /// <returns></returns>
        public Response<ReferralViewModel> AddReferral(ReferralViewModel referral)
        {
            const string apiUrl = baseRoute + "AddReferral";
            var response = communicationManager.Post<ReferralModel, Response<ReferralModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates referral
        /// </summary>
        /// <param name="referral">referral model</param>
        /// <returns></returns>
        public Response<ReferralViewModel> UpdateReferral(ReferralViewModel referral)
        {
            const string apiUrl = baseRoute + "UpdateReferral";
            var response = communicationManager.Put<ReferralModel, Response<ReferralModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes referral
        /// </summary>
        /// <param name="id">Referral Id</param>
        /// <returns></returns>
        public Response<ReferralViewModel> DeleteReferral(long id, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteReferral";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<ReferralModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates referral contact
        /// </summary>
        /// <param name="referralContact"></param>
        /// <returns></returns>
        public Response<ReferralContactViewModel> UpdateReferralContact(ReferralContactViewModel referralContact)
        {
            const string apiUrl = baseRoute + "UpdateReferralContact";
            var response = communicationManager.Put<ReferralContactModel, Response<ReferralContactModel>>(referralContact.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes referral contact
        /// </summary>
        /// <param name="referralContactId"></param>
        /// <returns></returns>
        public Response<ReferralContactViewModel> DeleteReferalContact(long referralContactId, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteReferalContact";
            var requestId = new NameValueCollection { { "referralContactId", referralContactId.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<ReferralContactModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}
