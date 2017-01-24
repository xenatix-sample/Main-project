using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical.SocialRelationship;
using Axis.Model.Clinical.SocialRelationship;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Clinical
{
    /// <summary>
    ///
    /// </summary>
    public class SocialRelationshipController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The srh data provider
        /// </summary>
        private readonly ISocialRelationshipDataProvider _socialRelationshipDataProvider;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRelationshipController"/> class.
        /// </summary>
        /// <param name="socialRelationshipDataProvider">The srh data provider.</param>
        public SocialRelationshipController(ISocialRelationshipDataProvider socialRelationshipDataProvider)
        {
            this._socialRelationshipDataProvider = socialRelationshipDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the social relationships by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetSocialRelationshipsByContact(long contactID)
        {
            return new HttpResult<Response<SocialRelationshipModel>>(_socialRelationshipDataProvider.GetSocialRelationshipsByContact(contactID), Request);
        }

        /// <summary>
        /// Adds the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddSocialRelationship(SocialRelationshipModel model)
        {
            return new HttpResult<Response<SocialRelationshipModel>>(_socialRelationshipDataProvider.AddSocialRelationship(model), Request);
        }

        /// <summary>
        /// Updates the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateSocialRelationship(SocialRelationshipModel model)
        {
            return new HttpResult<Response<SocialRelationshipModel>>(_socialRelationshipDataProvider.UpdateSocialRelationship(model), Request);
        }

        /// <summary>
        /// Deletes the social relationship.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteSocialRelationship(long ID, DateTime modifiedOn)
        {
            return new HttpResult<Response<SocialRelationshipModel>>(_socialRelationshipDataProvider.DeleteSocialRelationship(ID, modifiedOn), Request);
        }

        #endregion Public Methods
    }
}