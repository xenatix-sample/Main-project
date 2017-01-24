using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;

namespace Axis.Service.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "registration/";

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationService"/> class.
        /// </summary>
        public RegistrationService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> GetContactDemographics(long contactID)
        {
            string apiUrl = BaseRoute + "GetContactDemographics";
            var requestXMLValueNvc = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<ContactDemographicsModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the Contact Address.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactAddressModel> GetContactAddress(long contactID)
        {
            string apiUrl = BaseRoute + "GetContactAddress";
            var requestXMLValueNvc = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<ContactAddressModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> AddContactDemographics(ContactDemographicsModel contact)
        {
            string apiUrl = BaseRoute + "AddContactDemographics";
            return _communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(contact, apiUrl);
        }

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> UpdateContactDemographics(ContactDemographicsModel contact)
        {
            string apiUrl = BaseRoute + "UpdateContactDemographics";
            return _communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(contact, apiUrl);
        }

        /// <summary>
        /// Get the contact/client details based on search criteria
        /// </summary>
        /// <param name="SearchCriteria">Text to Search</param>
        /// <param name="contactType">contact type of contact</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> GetClientSummary(string SearchCriteria, string ContactType)
        {
            string apiUrl = BaseRoute + "GetClientSummary";
            SearchCriteria = HttpUtility.UrlEncode(SearchCriteria);
            var request = new NameValueCollection { { "SearchCriteria", SearchCriteria }, { "ContactType", ContactType ?? string.Empty } };
            return _communicationManager.Get<Response<ContactDemographicsModel>>(request, apiUrl);
        }

        /// <summary>
        /// Verifies the duplicate contacts.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> VerifyDuplicateContacts(ContactDemographicsModel contact)
        {
            string apiUrl = BaseRoute + "VerifyDuplicateContacts";
            return _communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(contact, apiUrl);
        }
        /// <summary>
        /// Get SSN by using contact id
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        public Response<String> GetSSN(long contactID)
        {
            string apiUrl = BaseRoute + "GetSSN";
            var request = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<String>>(request, apiUrl);
        }
    }
}