using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration
{
    /// <summary>
    /// Contact Relationship Service class to call the web api methods
    /// </summary>
    /// <seealso cref="Axis.Service.Registration.IContactRelationshipService" />
    public class ContactRelationshipService : IContactRelationshipService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ContactRelationship/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ContactRelationshipService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">The token.</param>
        public ContactRelationshipService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact Relationship.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactRelationshipModel> GetContactRelationship(long contactID, long parentContactID)
        {
            const string apiUrl = BaseRoute + "GetContactRelationship";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) }, { "parentContactID", parentContactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactRelationshipModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        public Response<ContactRelationshipModel> AddContactRelationship(List<ContactRelationshipModel> contactRelationship)
        {
            const string apiUrl = BaseRoute + "AddContactRelationship";
            return communicationManager.Post<List<ContactRelationshipModel>, Response<ContactRelationshipModel>>(contactRelationship, apiUrl);
        }

        /// <summary>
        /// Updates the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        public Response<ContactRelationshipModel> UpdateContactRelationship(List<ContactRelationshipModel> contactRelationship)
        {
            const string apiUrl = BaseRoute + "UpdateContactRelationship";
            return communicationManager.Put<List<ContactRelationshipModel>, Response<ContactRelationshipModel>>(contactRelationship, apiUrl);
        }

        /// <summary>
        /// Deletes the contact Relationship.
        /// </summary>
        /// <param name="contactRelationshipTypeID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<ContactRelationshipModel> DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteContactRelationship";
            var requestId = new NameValueCollection
            {
                { "contactRelationshipTypeID", contactRelationshipTypeID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ContactRelationshipModel>>(requestId, apiUrl);
        }
    }
}