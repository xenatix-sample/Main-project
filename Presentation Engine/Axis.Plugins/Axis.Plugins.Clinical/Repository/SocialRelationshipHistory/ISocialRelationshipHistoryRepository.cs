using System;
using Axis.Model.Common;
using System.Threading.Tasks;
using Axis.Model.Clinical;

namespace Axis.Plugins.Clinical
{
    public interface ISocialRelationshipHistoryRepository
    {
        Task<Response<SocialRelationshipHistoryViewModel>> GetSocialRelationshipAsync(long socialRelationshipID);
        Task<Response<SocialRelationshipHistoryDetailsModel>> GetSocialRelationshipDetailsAsync(long socialRelationshipID);

        /// <summary>
        /// Add social relationship history for contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Response<SocialRelationshipHistoryViewModel> AddSocialRelationHistory(SocialRelationshipHistoryViewModel model);

        /// <summary>
        /// Update social relationship history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Response<SocialRelationshipHistoryViewModel> UpdateSocialRelationHistory(SocialRelationshipHistoryViewModel model);

        /// <summary>
        /// Remove social relationship history 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Response<SocialRelationshipHistoryViewModel> DeleteSocialRelationHistory(long Id, DateTime modifiedOn);
        Response<SocialRelationshipHistoryDetailsModel> AddSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model);
        Response<SocialRelationshipHistoryDetailsModel> UpdateSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model);
    }
}
