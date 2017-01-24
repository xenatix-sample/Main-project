using Axis.Model.Clinical.SocialRelationship;
using Axis.Model.Common;
using Axis.Service.Clinical.SocialRelationship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Clinical.SocialRelationship
{
    public class SocialRelationshipRuleEngine : ISocialRelationshipRuleEngine
    {
         #region Class Variables

        /// <summary>
        /// The srh service
        /// </summary>
        private readonly  ISocialRelationshipService _socialRelationshipService;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRelationshipRuleEngine"/> class.
        /// </summary>
        /// <param name="socialRelationshipService">The Social Relationship Service .</param>
        public SocialRelationshipRuleEngine(ISocialRelationshipService socialRelationshipService)
        {
            this._socialRelationshipService = socialRelationshipService;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the social relationships  by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<SocialRelationshipModel> GetSocialRelationshipsByContact(long contactID)
        {
            return _socialRelationshipService.GetSocialRelationshipsByContact(contactID);
        }

        
        /// <summary>
        /// Adds the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        public Response<SocialRelationshipModel> AddSocialRelationship(SocialRelationshipModel model)
        {
            return _socialRelationshipService.AddSocialRelationship(model);
        }

        /// <summary>
        /// Updates the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        public Response<SocialRelationshipModel> UpdateSocialRelationship(SocialRelationshipModel model)
        {
            return _socialRelationshipService.UpdateSocialRelationship(model);
        }

        /// <summary>
        /// Deletes the social relationship.
        /// </summary>
        /// <param name="srhID">The srh identifier.</param>
        /// <returns></returns>
        public Response<SocialRelationshipModel> DeleteSocialRelationship(long srhID, DateTime modifiedOn)
        {
            return _socialRelationshipService.DeleteSocialRelationship(srhID, modifiedOn);
        }

        #endregion Public Methods
    }
}
