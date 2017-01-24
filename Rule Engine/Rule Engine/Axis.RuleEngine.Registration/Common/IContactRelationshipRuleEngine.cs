using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactRelationshipRuleEngine
    {
        /// <summary>
        /// Gets the contact Relationship.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactRelationshipModel> GetContactRelationship(long contactID, long parentContactID);

        /// <summary>
        /// Adds the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        Response<ContactRelationshipModel> AddContactRelationship(List<ContactRelationshipModel> contactRelationship);

        /// <summary>
        /// Updates the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        Response<ContactRelationshipModel> UpdateContactRelationship(List<ContactRelationshipModel> contactRelationship);

        /// <summary>
        /// Deletes the contact Relationship.
        /// </summary>
        /// <param name="contactRelationshipTypeID">The contact Relationship identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<ContactRelationshipModel> DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn);
    }
}