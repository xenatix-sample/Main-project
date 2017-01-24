using Axis.Model.Clinical.SocialRelationship;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Clinical.SocialRelationship
{
   public interface ISocialRelationshipRuleEngine
    {
        /// <summary>
        /// Gets the social relationship by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<SocialRelationshipModel> GetSocialRelationshipsByContact(long contactID);

        
        /// <summary>
        /// Adds the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        Response<SocialRelationshipModel> AddSocialRelationship(SocialRelationshipModel model);

        /// <summary>
        /// Updates the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        Response<SocialRelationshipModel> UpdateSocialRelationship(SocialRelationshipModel model);

        /// <summary>
        /// Deletes the social relationship.
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns></returns>
        Response<SocialRelationshipModel> DeleteSocialRelationship(long ID, DateTime modifiedOn);
    }
}
