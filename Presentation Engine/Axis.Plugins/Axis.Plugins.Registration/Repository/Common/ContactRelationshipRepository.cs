using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    /// Repository for contact Relationship to call web api methods.
    /// </summary>
    /// <seealso cref="Axis.Plugins.Registration.IContactRelationshipRepository" />
    public class ContactRelationshipRepository : IContactRelationshipRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ContactRelationship/";

        /// <summary>
        /// constructor
        /// </summary>
        public ContactRelationshipRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="token">token</param>
        public ContactRelationshipRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact Relationship.
        /// </summary>
        /// <param name="contactID">Contact Id of contact</par1am>
        /// <returns>
        /// Response of type ContactEmailModel
        /// </returns>
        public Response<ContactRelationshipViewModel> GetContactRelationship(long contactID, long parentContactID)
        {
            const string apiUrl = baseRoute + "GetContactRelationship";
            var param = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) }, { "parentContactID", parentContactID.ToString(CultureInfo.InvariantCulture) } };
            var retVal = _communicationManager.Get<Response<ContactRelationshipModel>>(param, apiUrl);
            return retVal.ToViewModel();
        }

        /// <summary>
        /// Adds the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        public Response<ContactRelationshipViewModel> AddContactRelationship(List<ContactRelationshipViewModel> contactRelationship)
        {
            const string apiUrl = baseRoute + "AddContactRelationship";

            var response = _communicationManager.Post<List<ContactRelationshipModel>, Response<ContactRelationshipModel>>(contactRelationship.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        public Response<ContactRelationshipViewModel> UpdateContactRelationship(List<ContactRelationshipViewModel> contactRelationship)
        {
            const string apiUrl = baseRoute + "UpdateContactRelationship";

            var response = _communicationManager.Put<List<ContactRelationshipModel>, Response<ContactRelationshipModel>>(contactRelationship.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the contact Relationship.
        /// </summary>
        /// <param name="contactRelationshipTypeID">The contact Relationship identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<ContactRelationshipViewModel> DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteContactRelationship";
            var requestId = new NameValueCollection { { "contactRelationshipTypeID", contactRelationshipTypeID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Delete<Response<ContactRelationshipModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}