using Axis.Configuration;
using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Clinical
{
    public class SocialRelationshipHistoryService : ISocialRelationshipHistoryService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "SocialRelationshipHistory/";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialRelationshipHistoryService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public SocialRelationshipHistoryService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the individual social relationship record
        /// </summary>
        /// <param name="socialRelationshipID"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> GetSocialRelationship(long socialRelationshipID)
        {
            const string apiUrl = BaseRoute + "GetSocialRelationship";
            var param = new NameValueCollection { { "socialRelationshipID", socialRelationshipID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<SocialRelationshipHistoryModel>>(param, apiUrl);
        }

        /// <summary>
        /// Add social relationship history for contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> AddSocialRelationHistory(SocialRelationshipHistoryModel model)
        {
            const string apiUrl = BaseRoute + "AddSocialRelationHistory";
            return _communicationManager.Post<SocialRelationshipHistoryModel, Response<SocialRelationshipHistoryModel>>(model, apiUrl);
        }

        /// <summary>
        /// Update social relationship history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> UpdateSocialRelationHistory(SocialRelationshipHistoryModel model)
        {
            const string apiUrl = BaseRoute + "UpdateSocialRelationHistory";
            return _communicationManager.Put<SocialRelationshipHistoryModel, Response<SocialRelationshipHistoryModel>>(model, apiUrl);
        }

        public Response<SocialRelationshipHistoryDetailsModel> AddSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            const string apiUrl = BaseRoute + "AddSocialRelationshipDetail";
            return _communicationManager.Post<SocialRelationshipHistoryDetailsModel, Response<SocialRelationshipHistoryDetailsModel>>(model, apiUrl);
        }

        public Response<SocialRelationshipHistoryDetailsModel> UpdateSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            const string apiUrl = BaseRoute + "UpdateSocialRelationshipDetail";
            return _communicationManager.Put<SocialRelationshipHistoryDetailsModel, Response<SocialRelationshipHistoryDetailsModel>>(model, apiUrl);
        }

        /// <summary>
        /// Remove social relationship history 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> DeleteSocialRelationHistory(long Id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteSocialRelationHistory";
            var requestId = new NameValueCollection { { "Id", Id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Delete<Response<SocialRelationshipHistoryModel>>(requestId, apiUrl);
        }

        #endregion
    }
}
