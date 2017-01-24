using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactRelationshipRepository
    {
        /// <summary>
        /// Gets the contact Relationship.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactRelationshipViewModel> GetContactRelationship(long contactID, long parentContactID);

        /// <summary>
        /// Adds the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        Response<ContactRelationshipViewModel> AddContactRelationship(List<ContactRelationshipViewModel> contactRelationship);

        /// <summary>
        /// Updates the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        Response<ContactRelationshipViewModel> UpdateContactRelationship(List<ContactRelationshipViewModel> contactRelationship);

        /// <summary>
        /// Deletes the contact Relationship.
        /// </summary>
        /// <param name="contactRelationshipTypeID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<ContactRelationshipViewModel> DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn);
    }
}