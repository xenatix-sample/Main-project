using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// Api Controller for Contact Relationship
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseApiController" />
    public class ContactRelationshipController : BaseApiController
    {
        /// <summary>
        /// The contact Relationship repository
        /// </summary>
        private readonly IContactRelationshipRepository _contactRelationshipRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRelationshipController" /> class.
        /// </summary>
        /// <param name="contactRelationshipRepository">The contact Relationship repository.</param>
        public ContactRelationshipController(IContactRelationshipRepository contactRelationshipRepository)
        {
            this._contactRelationshipRepository = contactRelationshipRepository;
        }

        /// <summary>
        /// Gets the contact Relationship.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactRelationshipViewModel> GetContactRelationship(long contactID, long parentContactID)
        {
            return _contactRelationshipRepository.GetContactRelationship(contactID, parentContactID);
        }

        /// <summary>
        /// Adds the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ContactRelationshipViewModel> AddContactRelationship(ContactRelationshipViewModel contactRelationship)
        {
            return _contactRelationshipRepository.AddContactRelationship(new List<ContactRelationshipViewModel> { contactRelationship });
        }

        /// <summary>
        /// Updates the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ContactRelationshipViewModel> UpdateContactRelationship(ContactRelationshipViewModel contactRelationship)
        {
            return _contactRelationshipRepository.UpdateContactRelationship(new List<ContactRelationshipViewModel> { contactRelationship });
        }

        /// <summary>
        /// Deletes the contact Relationship.
        /// </summary>
        /// <param name="contactRelationshipTypeID">The contact Relationship identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ContactRelationshipViewModel> DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn)
        {
            return _contactRelationshipRepository.DeleteContactRelationship(contactRelationshipTypeID, modifiedOn);
        }
    }
}