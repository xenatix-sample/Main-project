using System;
using Axis.Plugins.Clinical.Models.SocialRelationship;
using Axis.Plugins.Clinical.Repository.SocialRelationship;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Clinical.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class SocialRelationshipController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The social relationship  repository
        /// </summary>
        private readonly ISocialRelationshipRepository _socialRelationshipRepository;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRelationshipController"/> class.
        /// </summary>
        /// <param name="socialRelationshipRepository">The social relationship  repository.</param>
        public SocialRelationshipController(ISocialRelationshipRepository socialRelationshipRepository)
        {
            this._socialRelationshipRepository = socialRelationshipRepository;
        }

        #endregion Constructors

        #region json Results
        /// <summary>
        /// Gets the social relationship  by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<SocialRelationshipViewModel> GetSocialRelationshipsByContact(long contactID)
        {
            return _socialRelationshipRepository.GetSocialRelationshipsByContact(contactID);
        }


        /// <summary>
        /// Add social relationship for contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<SocialRelationshipViewModel> AddSocialRelationship(SocialRelationshipViewModel model)
        {
            model.TakenTime = model.TakenTime.ToUniversalTime();
            return _socialRelationshipRepository.AddSocialRelationship(model);
        }

        /// <summary>
        /// Update social relationship history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public Response<SocialRelationshipViewModel> UpdateSocialRelationship(SocialRelationshipViewModel model)
        {
            model.TakenTime = model.TakenTime.ToUniversalTime();
            return _socialRelationshipRepository.UpdateSocialRelationship(model);
        }


        /// <summary>
        /// Remove social relationship
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<SocialRelationshipViewModel> DeleteSocialRelationship(long Id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _socialRelationshipRepository.DeleteSocialRelationship(Id, modifiedOn);
        }
        #endregion Json Results
    }
}
