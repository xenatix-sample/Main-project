using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Service.Clinical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Clinical
{
    public class SocialRelationshipHistoryRuleEngine : ISocialRelationshipHistoryRuleEngine
    {
        #region Class Variables

        readonly ISocialRelationshipHistoryService _socialRelationshipHistoryService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socialRelationshipHistoryService"></param>
        public SocialRelationshipHistoryRuleEngine(ISocialRelationshipHistoryService socialRelationshipHistoryService)
        {
            _socialRelationshipHistoryService = socialRelationshipHistoryService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the data for the individual social relationship record
        /// </summary>
        /// <param name="socialRelationshipID"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> GetSocialRelationship(long socialRelationshipID)
        {
            return _socialRelationshipHistoryService.GetSocialRelationship(socialRelationshipID);
        }

        /// <summary>
        /// Add social relationship history for contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> AddSocialRelationHistory(SocialRelationshipHistoryModel model)
        {
            return _socialRelationshipHistoryService.AddSocialRelationHistory(model);
        }

        /// <summary>
        /// Update social relationship history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> UpdateSocialRelationHistory(SocialRelationshipHistoryModel model)
        {
            return _socialRelationshipHistoryService.UpdateSocialRelationHistory(model);
        }

        public Response<SocialRelationshipHistoryDetailsModel> AddSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            return _socialRelationshipHistoryService.AddSocialRelationshipDetail(model);
        }

        public Response<SocialRelationshipHistoryDetailsModel> UpdateSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            return _socialRelationshipHistoryService.UpdateSocialRelationshipDetail(model);
        }

        /// <summary>
        /// Remove social relationship history 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> DeleteSocialRelationHistory(long Id, DateTime modifiedOn)
        {
            return _socialRelationshipHistoryService.DeleteSocialRelationHistory(Id, modifiedOn);
        }

        #endregion
    }
}
