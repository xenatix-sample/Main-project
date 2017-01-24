using System;
using Axis.Model.Common;
using Axis.RuleEngine.Clinical;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Helpers.Validation;
using Axis.Model.Clinical;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Clinical
{
    /// <summary>
    /// Controller for SocialRelationshipHistory
    /// </summary>
    public class SocialRelationshipHistoryController : BaseApiController
    {
        #region Class Variables

        readonly ISocialRelationshipHistoryRuleEngine _socialRelationshipHistory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socialRelationshipHistory"></param>
        public SocialRelationshipHistoryController(ISocialRelationshipHistoryRuleEngine socialRelationshipHistory)
        {
            this._socialRelationshipHistory = socialRelationshipHistory;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the data for the individual social relationship record
        /// </summary>
        /// <param name="socialRelationshipID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_SocialRelationshipHistory_SocialRelationshipHistory, Permission = Permission.Read)]
        public IHttpActionResult GetSocialRelationship(long socialRelationshipID)
        {
            return new HttpResult<Response<SocialRelationshipHistoryModel>>(_socialRelationshipHistory.GetSocialRelationship(socialRelationshipID), Request);
        }

        /// <summary>
        /// Add social relationship history for contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_SocialRelationshipHistory_SocialRelationshipHistory, Permission = Permission.Create)]
        public IHttpActionResult AddSocialRelationHistory(SocialRelationshipHistoryModel model)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<SocialRelationshipHistoryModel>>(_socialRelationshipHistory.AddSocialRelationHistory(model), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<SocialRelationshipHistoryModel>() { DataItems = new List<SocialRelationshipHistoryModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<SocialRelationshipHistoryModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Update social relationship history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_SocialRelationshipHistory_SocialRelationshipHistory, Permission = Permission.Update)]
        public IHttpActionResult UpdateSocialRelationHistory(SocialRelationshipHistoryModel model)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<SocialRelationshipHistoryModel>>(_socialRelationshipHistory.UpdateSocialRelationHistory(model), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<SocialRelationshipHistoryModel>() { DataItems = new List<SocialRelationshipHistoryModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<SocialRelationshipHistoryModel>>(validationResponse, Request);
            }
        }

        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_SocialRelationshipHistory_SocialRelationshipHistory, Permission = Permission.Create)]
        public IHttpActionResult AddSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            return  new HttpResult<Response<SocialRelationshipHistoryDetailsModel>>(_socialRelationshipHistory.AddSocialRelationshipDetail(model), Request);
        }

        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_SocialRelationshipHistory_SocialRelationshipHistory, Permission = Permission.Update)]
        public IHttpActionResult UpdateSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            return new HttpResult<Response<SocialRelationshipHistoryDetailsModel>>(_socialRelationshipHistory.UpdateSocialRelationshipDetail(model), Request);
        }

        /// <summary>
        /// Remove social relationship history 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_SocialRelationshipHistory_SocialRelationshipHistory, Permission = Permission.Delete)]
        public IHttpActionResult DeleteSocialRelationHistory(long Id, DateTime modifiedOn)
        {
            return new HttpResult<Response<SocialRelationshipHistoryModel>>(_socialRelationshipHistory.DeleteSocialRelationHistory(Id, modifiedOn), Request);
        }

        #endregion
    }
}
