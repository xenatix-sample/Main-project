using System;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Clinical;

namespace Axis.Plugins.Clinical.ApiControllers
{
    public class SocialRelationshipHistoryController : BaseApiController
    {
        readonly ISocialRelationshipHistoryRepository _socialRelationshipHistoryRepository;

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socialRelationshipHistoryRepository"></param>
        public SocialRelationshipHistoryController(ISocialRelationshipHistoryRepository socialRelationshipHistoryRepository)
        {
            _socialRelationshipHistoryRepository = socialRelationshipHistoryRepository;
        }

        #endregion

        #region Json Results

        /// <summary>
        /// Get the bundle data for the social relationship record
        /// </summary>
        /// <param name="socialRelationshipID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<SocialRelationshipHistoryViewModel>> GetSocialRelationship(long socialRelationshipID)
        {
            var result = await _socialRelationshipHistoryRepository.GetSocialRelationshipAsync(socialRelationshipID);
            return result;
        }

        /// <summary>
        /// get all of the detail records for the social relationship bundle
        /// </summary>
        /// <param name="socialRelationshipID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<SocialRelationshipHistoryDetailsModel>> GetSocialRelationshipDetails(long socialRelationshipID)
        {
            var result = await _socialRelationshipHistoryRepository.GetSocialRelationshipDetailsAsync(socialRelationshipID);
            return result;
        }

        /// <summary>
        /// Add social relationship history for contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<SocialRelationshipHistoryViewModel> AddSocialRelationHistory(SocialRelationshipHistoryViewModel model)
        {
            model.TakenTime = model.TakenTime.ToUniversalTime();
            return _socialRelationshipHistoryRepository.AddSocialRelationHistory(model);
        }

        /// <summary>
        /// Update social relationship history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public Response<SocialRelationshipHistoryViewModel> UpdateSocialRelationHistory(SocialRelationshipHistoryViewModel model)
        {
            model.TakenTime = model.TakenTime.ToUniversalTime();
            return _socialRelationshipHistoryRepository.UpdateSocialRelationHistory(model);
        }

        [HttpPost]
        public Response<SocialRelationshipHistoryDetailsModel> AddSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            return _socialRelationshipHistoryRepository.AddSocialRelationshipDetail(model);
        }

        [HttpPut]
        public Response<SocialRelationshipHistoryDetailsModel> UpdateSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            return _socialRelationshipHistoryRepository.UpdateSocialRelationshipDetail(model);
        }

        /// <summary>
        /// Remove social relationship history 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        //[UTCDate(modifiedOn)]
        [HttpDelete]
        public Response<SocialRelationshipHistoryViewModel> DeleteSocialRelationHistory(long Id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _socialRelationshipHistoryRepository.DeleteSocialRelationHistory(Id, modifiedOn);
        }

        #endregion
    }
}
