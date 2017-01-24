using System;
using Axis.Model.Clinical;
using Axis.Model.Common;

namespace Axis.RuleEngine.Clinical
{
    public interface ISocialRelationshipHistoryRuleEngine
    {
        /// <summary>
        /// Get the data for the individual social relationship record
        /// </summary>
        /// <param name="socialRelationshipID"></param>
        /// <returns></returns>
        Response<SocialRelationshipHistoryModel> GetSocialRelationship(long socialRelationshipID);

        /// <summary>
        /// Add social relationship history for contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Response<SocialRelationshipHistoryModel> AddSocialRelationHistory(SocialRelationshipHistoryModel model);

        /// <summary>
        /// Update social relationship history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Response<SocialRelationshipHistoryModel> UpdateSocialRelationHistory(SocialRelationshipHistoryModel model);

        Response<SocialRelationshipHistoryDetailsModel> AddSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model);
        Response<SocialRelationshipHistoryDetailsModel> UpdateSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model);

        /// <summary>
        /// Remove social relationship history 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Response<SocialRelationshipHistoryModel> DeleteSocialRelationHistory(long Id, DateTime modifiedOn);
    }
}
