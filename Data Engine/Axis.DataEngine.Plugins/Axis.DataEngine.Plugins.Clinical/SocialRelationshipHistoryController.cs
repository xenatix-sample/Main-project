using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical;
using Axis.Model.Clinical;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Clinical
{
    public class SocialRelationshipHistoryController : BaseApiController
    {
        #region Class Variables

        readonly ISocialRelationshipHistoryDataProvider _socialRelationshipHistoryDataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="noteDataProvider"></param>
        public SocialRelationshipHistoryController(ISocialRelationshipHistoryDataProvider socialRelationshipHistoryDataProvider)
        {
            _socialRelationshipHistoryDataProvider = socialRelationshipHistoryDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the individual social relationship record
        /// </summary>
        /// <param name="socialRelationshipID"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSocialRelationship(long socialRelationshipID)
        {
            return new HttpResult<Response<SocialRelationshipHistoryModel>>(_socialRelationshipHistoryDataProvider.GetSocialRelationship(socialRelationshipID), Request);
        }

        /// <summary>
        /// Add social relationship history for contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddSocialRelationHistory(SocialRelationshipHistoryModel model)
        {
            return new HttpResult<Response<SocialRelationshipHistoryModel>>(_socialRelationshipHistoryDataProvider.AddSocialRelationHistory(model), Request);
        }

        /// <summary>
        /// Update social relationship history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateSocialRelationHistory(SocialRelationshipHistoryModel model)
        {
            return new HttpResult<Response<SocialRelationshipHistoryModel>>(_socialRelationshipHistoryDataProvider.UpdateSocialRelationHistory(model), Request);
        }

        [HttpPost]
        public IHttpActionResult AddSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            return new HttpResult<Response<SocialRelationshipHistoryDetailsModel>>(_socialRelationshipHistoryDataProvider.AddSocialRelationshipDetail(model), Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            return new HttpResult<Response<SocialRelationshipHistoryDetailsModel>>(_socialRelationshipHistoryDataProvider.UpdateSocialRelationshipDetail(model), Request);
        }

        /// <summary>
        /// Remove social relationship history 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteSocialRelationHistory(long Id, DateTime modifiedOn)
        {
            return new HttpResult<Response<SocialRelationshipHistoryModel>>(_socialRelationshipHistoryDataProvider.DeleteSocialRelationHistory(Id, modifiedOn), Request);
        }

        #endregion
    }
}
