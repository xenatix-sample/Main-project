using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;

namespace Axis.DataProvider.Registration.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactRelationshipDataProvider
    {
        /// <summary>
        /// Gets the contact Relationship.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactRelationshipModel> GetContactRelationship(long contactID, long parentContactID, long? referralHeaderID = null);

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
        /// <param name="ContactRelationshipTypeID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<ContactRelationshipModel> DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn);
    }
}