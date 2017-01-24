using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    /// Rule engine class for calling service method of contact Relationship Service
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Registration.IContactRelationshipRuleEngine" />
    public class ContactRelationshipRuleEngine : IContactRelationshipRuleEngine
    {
        /// <summary>
        /// The contact Relationship service
        /// </summary>
        private readonly IContactRelationshipService _contactRelationshipService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRelationshipRuleEngine"/> class.
        /// </summary>
        /// <param name="contactRelationshipService">The contact Relationship service.</param>
        public ContactRelationshipRuleEngine(IContactRelationshipService contactRelationshipService)
        {
            this._contactRelationshipService = contactRelationshipService;
        }

        /// <summary>
        /// Gets the contact Relationship.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactRelationshipModel> GetContactRelationship(long contactID, long parentContactID)
        {
            return _contactRelationshipService.GetContactRelationship(contactID, parentContactID);
        }

        /// <summary>
        /// Adds the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        public Response<ContactRelationshipModel> AddContactRelationship(List<ContactRelationshipModel> contactRelationship)
        {
            return _contactRelationshipService.AddContactRelationship(contactRelationship);
        }

        /// <summary>
        /// Updates the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        public Response<ContactRelationshipModel> UpdateContactRelationship(List<ContactRelationshipModel> contactRelationship)
        {
            return _contactRelationshipService.UpdateContactRelationship(contactRelationship);
        }

        /// <summary>
        /// Deletes the contact Relationship.
        /// </summary>
        /// <param name="contactRelationshipTypeID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<ContactRelationshipModel> DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn)
        {
            return _contactRelationshipService.DeleteContactRelationship(contactRelationshipTypeID, modifiedOn);
        }
    }
}