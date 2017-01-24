using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;

namespace Axis.Service.Registration
{
    /// <summary>
    /// Interface of Contact Relationship Service class to call the web api methods
    /// </summary>
    public interface IContactRelationshipService
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
        /// <param name="contactRelationshipTypeID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<ContactRelationshipModel> DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn);
    }
}