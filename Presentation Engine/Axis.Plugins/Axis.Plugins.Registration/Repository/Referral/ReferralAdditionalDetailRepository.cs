using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Referrals;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Repository.Referral
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.Plugins.Registration.Repository.Referral.IReferralAdditionalDetailRepository" />
    public class ReferralAdditionalDetailRepository : IReferralAdditionalDetailRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ReferralAdditionalDetail/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralRepository" /> class.
        /// </summary>
        public ReferralAdditionalDetailRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReferralAdditionalDetailRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public async Task<Response<ReferralAdditionalDetailViewModel>> GetReferralAdditionalDetail(long contactID)
        {
            const string apiUrl = baseRoute + "GetReferralAdditionalDetail";
            var param = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = (await communicationManager.GetAsync<Response<ReferralAdditionalDetailModel>>(param, apiUrl));

            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public async Task<Response<ReferralDetailsViewModel>> GetReferralsDetails(long contactID)
        {
            const string apiUrl = baseRoute + "GetReferralsDetails";
            var param = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = (await communicationManager.GetAsync<Response<ReferralDetailsModel>>(param, apiUrl));
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the referral .
        /// </summary>
        /// <param name="referral">The referral .</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailViewModel> AddReferralAdditionalDetail(ReferralAdditionalDetailViewModel referral)
        {
            const string apiUrl = baseRoute + "AddReferralAdditionalDetail";
            var response = communicationManager.Post<ReferralAdditionalDetailModel, Response<ReferralAdditionalDetailModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the referral .
        /// </summary>
        /// <param name="referral">The referral .</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailViewModel> UpdateReferralAdditionalDetail(ReferralAdditionalDetailViewModel referral)
        {
            const string apiUrl = baseRoute + "UpdateReferralAdditionalDetail";
            var response = communicationManager.Put<ReferralAdditionalDetailModel, Response<ReferralAdditionalDetailModel>>(referral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the referral details.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReferralDetailsViewModel> DeleteReferralDetails(long contactID)
        {
            const string apiUrl = baseRoute + "DeleteReferralDetails";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<ReferralDetailsModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}
