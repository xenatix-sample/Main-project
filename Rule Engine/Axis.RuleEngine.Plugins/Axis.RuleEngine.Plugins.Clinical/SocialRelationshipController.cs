using Axis.Constant;
using Axis.Model.Clinical.SocialRelationship;
using Axis.Model.Common;
using Axis.RuleEngine.Clinical.SocialRelationship;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Clinical
{
    /// <summary>
    ///
    /// </summary>
    public class SocialRelationshipController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The Social Relationship rule engine
        /// </summary>
        private readonly ISocialRelationshipRuleEngine _socialRelationshipRuleEngine;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRelationshipsController"/> class.
        /// </summary>
        /// <param name="socialRelationshipRuleEngine">The srh rule engine.</param>
        public SocialRelationshipController(ISocialRelationshipRuleEngine socialRelationshipRuleEngine)
        {
            this._socialRelationshipRuleEngine = socialRelationshipRuleEngine;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the social relationships  by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_SocialRelationshipHistory_SocialRelationshipHistory, Permission = Permission.Read)]
        public IHttpActionResult GetSocialRelationshipsByContact(long contactID)
        {
            return new HttpResult<Response<SocialRelationshipModel>>(_socialRelationshipRuleEngine.GetSocialRelationshipsByContact(contactID), Request);
        }

        /// <summary>
        /// Adds the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_SocialRelationshipHistory_SocialRelationshipHistory, Permission = Permission.Create)]
        public IHttpActionResult AddSocialRelationship(SocialRelationshipModel model)
        {
            return new HttpResult<Response<SocialRelationshipModel>>(_socialRelationshipRuleEngine.AddSocialRelationship(model), Request);
        }

        /// <summary>
        /// Updates the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_SocialRelationshipHistory_SocialRelationshipHistory, Permission = Permission.Update)]
        public IHttpActionResult UpdateSocialRelationship(SocialRelationshipModel model)
        {
            return new HttpResult<Response<SocialRelationshipModel>>(_socialRelationshipRuleEngine.UpdateSocialRelationship(model), Request);
        }

        /// <summary>
        /// Deletes the social relationship.
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_SocialRelationshipHistory_SocialRelationshipHistory, Permission = Permission.Delete)]
        public IHttpActionResult DeleteSocialRelationship(long ID, DateTime modifiedOn)
        {
            return new HttpResult<Response<SocialRelationshipModel>>(_socialRelationshipRuleEngine.DeleteSocialRelationship(ID, modifiedOn), Request);
        }

        #endregion Public Methods
    }
}
