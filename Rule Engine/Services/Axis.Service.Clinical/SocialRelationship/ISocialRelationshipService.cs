using Axis.Model.Clinical.SocialRelationship;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Clinical.SocialRelationship
{
    public interface ISocialRelationshipService
    {
        /// <summary>
        /// Gets the social relationship by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<SocialRelationshipModel> GetSocialRelationshipsByContact(long contactID);

      

        /// <summary>
        /// Adds the .
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        Response<SocialRelationshipModel> AddSocialRelationship(SocialRelationshipModel model);

        /// <summary>
        /// Updates the .
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        Response<SocialRelationshipModel> UpdateSocialRelationship(SocialRelationshipModel model);

        /// <summary>
        /// Deletes the social relationship
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<SocialRelationshipModel> DeleteSocialRelationship(long ID, DateTime modifiedOn);
    }
}
