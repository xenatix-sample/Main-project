using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.SocialRelationship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Clinical.Repository.SocialRelationship
{
    public interface ISocialRelationshipRepository
    {
        /// <summary>
        /// Gets the social relationship by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<SocialRelationshipViewModel> GetSocialRelationshipsByContact(long contactID);

        /// <summary>
        /// Adds the social relationship.
        /// </summary>
        /// <param name="model">The ros.</param>
        /// <returns></returns>
        Response<SocialRelationshipViewModel> AddSocialRelationship(SocialRelationshipViewModel model);

        /// <summary>
        /// Updates the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipViewModel.</param>
        /// <returns></returns>
        Response<SocialRelationshipViewModel> UpdateSocialRelationship(SocialRelationshipViewModel model);

        /// <summary>
        /// Deletes the social relationship.
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns></returns>
        Response<SocialRelationshipViewModel> DeleteSocialRelationship(long ID, DateTime modifiedOn);

    }
}
