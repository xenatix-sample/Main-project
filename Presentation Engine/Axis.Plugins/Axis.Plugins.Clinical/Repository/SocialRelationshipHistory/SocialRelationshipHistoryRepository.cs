using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using Axis.Constant;
using System.Threading.Tasks;


namespace Axis.Plugins.Clinical
{
    public class SocialRelationshipHistoryRepository : ISocialRelationshipHistoryRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "SocialRelationshipHistory/";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRelationshipHistoryRepository" /> class.
        /// </summary>
        public SocialRelationshipHistoryRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRelationshipHistoryRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public SocialRelationshipHistoryRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion

        #region Async Methods

        public async Task<Response<SocialRelationshipHistoryViewModel>> GetSocialRelationshipAsync(long socialRelationshipID)
        {
            string apiUrl = baseRoute + "GetSocialRelationship";
            var param = new NameValueCollection();
            param.Add("socialRelationshipID", socialRelationshipID.ToString());
            var response = await _communicationManager.GetAsync<Response<SocialRelationshipHistoryModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        public async Task<Response<SocialRelationshipHistoryDetailsModel>> GetSocialRelationshipDetailsAsync(long socialRelationshipID)
        {
            string apiUrl = baseRoute + "GetSocialRelationship";
            var param = new NameValueCollection();
            param.Add("socialRelationshipID", socialRelationshipID.ToString());
            var response = await _communicationManager.GetAsync<Response<SocialRelationshipHistoryModel>>(param, apiUrl);
            var newResponse = response.CloneResponse<SocialRelationshipHistoryDetailsModel>();
            newResponse.DataItems = response.DataItems.First().Details;

            return newResponse;
        }

        #endregion  

        #region Public Methods

        /// <summary>
        /// Add social relationship history for contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryViewModel> AddSocialRelationHistory(SocialRelationshipHistoryViewModel model)
        {
            string apiUrl = baseRoute + "AddSocialRelationHistory";
            var response = _communicationManager.Post<SocialRelationshipHistoryModel, Response<SocialRelationshipHistoryModel>>(model.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Update social relationship history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryViewModel> UpdateSocialRelationHistory(SocialRelationshipHistoryViewModel model)
        {
            string apiUrl = baseRoute + "UpdateSocialRelationHistory";
            var response = _communicationManager.Put<SocialRelationshipHistoryModel, Response<SocialRelationshipHistoryModel>>(model.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        public Response<SocialRelationshipHistoryDetailsModel> AddSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            string apiUrl = baseRoute + "AddSocialRelationshipDetail";
            var response = _communicationManager.Post<SocialRelationshipHistoryDetailsModel, Response<SocialRelationshipHistoryDetailsModel>>(model, apiUrl);

            return response;
        }

        public Response<SocialRelationshipHistoryDetailsModel> UpdateSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            string apiUrl = baseRoute + "UpdateSocialRelationshipDetail";
            var response = _communicationManager.Put<SocialRelationshipHistoryDetailsModel, Response<SocialRelationshipHistoryDetailsModel>>(model, apiUrl);

            return response;
        }

        /// <summary>
        /// Remove social relationship history 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryViewModel> DeleteSocialRelationHistory(long Id, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"Id", Id.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return _communicationManager.Delete<Response<SocialRelationshipHistoryModel>>(param, apiUrl).ToViewModel();
        }

        #endregion
    }
}
