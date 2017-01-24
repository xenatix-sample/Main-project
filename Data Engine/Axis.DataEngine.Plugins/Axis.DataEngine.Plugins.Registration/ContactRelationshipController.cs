using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Common;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class ContactRelationshipController : BaseApiController
    {
        /// <summary>
        /// The contact Relationship data provider
        /// </summary>
        private IContactRelationshipDataProvider _contactRelationshipDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRelationshipController" /> class.
        /// </summary>
        /// <param name="ContactRelationshipDataProvider">The address provider.</param>
        public ContactRelationshipController(IContactRelationshipDataProvider contactRelationshipDataProvider)
        {
            this._contactRelationshipDataProvider = contactRelationshipDataProvider;
        }

        /// <summary>
        /// Gets the contact Relationship.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="parentContactID">The parent contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactRelationship(long contactID,long parentContactID)
        {
            return new HttpResult<Response<ContactRelationshipModel>>(_contactRelationshipDataProvider.GetContactRelationship(contactID, parentContactID), Request);
        }

        /// <summary>
        /// Adds the contact Relationship.
        /// </summary>
        /// <param name="ContactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddContactRelationship(List<ContactRelationshipModel> contactRelationship)
        {
            return new HttpResult<Response<ContactRelationshipModel>>(_contactRelationshipDataProvider.AddContactRelationship(contactRelationship), Request);
        }

        /// <summary>
        /// Updates the contact Relationship.
        /// </summary>
        /// <param name="ContactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateContactRelationship(List<ContactRelationshipModel> contactRelationship)
        {
            return new HttpResult<Response<ContactRelationshipModel>>(_contactRelationshipDataProvider.UpdateContactRelationship(contactRelationship), Request);
        }

        /// <summary>
        /// Deletes the contact Relationship.
        /// </summary>
        /// <param name="contactRelationshipTypeID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactRelationshipModel>>(_contactRelationshipDataProvider.DeleteContactRelationship(contactRelationshipTypeID, modifiedOn), Request);
        }
    }
}