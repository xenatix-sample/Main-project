using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Plugins.Registration.Models;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Axis.Plugins.Registration.Translator;
using Axis.Constant;


namespace Axis.Plugins.Registration.Repository.Referrals
{
    /// <summary>
    /// ReferralSearch Repository
    /// </summary>
    /// <seealso cref="Axis.Plugins.Registration.Repository.Referrals.IReferralSearchRepository" />
    public class ReferralSearchRepository : IReferralSearchRepository
    {
        private readonly CommunicationManager _communicationManager;
        private const string baseRoute = "ReferralSearch/";

        public ReferralSearchRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public ReferralSearchRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referrals asynchronous.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        
        public async Task<Response<ReferralSearchViewModel>> GetReferralsAsync(string searchStr, int searchType, long userID)
        {
            const string apiUrl = baseRoute + "GetReferrals";
            var param = new NameValueCollection { { "searchStr", searchStr.ToString(CultureInfo.InvariantCulture) }, { "searchType", searchType.ToString(CultureInfo.InvariantCulture) }, { "userID", userID.ToString(CultureInfo.InvariantCulture) } };
            var response = await _communicationManager.GetAsync<Response<ReferralSearchModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the referral.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
       
        public Response<ReferralSearchViewModel> DeleteReferral(long id, string reasonForDelete, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteReferral";
            var requestId = new NameValueCollection { 
                                { "id", id.ToString(CultureInfo.InvariantCulture) }, 
                                { "reasonForDelete", reasonForDelete.ToString(CultureInfo.InvariantCulture) },
                                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } }; 
            var response = _communicationManager.Delete<Response<ReferralSearchModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}
