using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;


namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    /// Registration repository to calling rule engine api
    /// </summary>
    public class RegistrationRepository : IRegistrationRepository
    {
        #region Private Variables

        private readonly CommunicationManager _communicationManager;
        private const string baseRoute = "Registration/";
        private const string getContactDemographics = "GetContactDemographics";
        private const string getContactAddress = "GetContactAddress";
        private const string addContactDemographics = "AddContactDemographics";
        private const string updateContactDemographics = "UpdateContactDemographics";
        private const string verifyDuplicateContacts = "VerifyDuplicateContacts";

        #endregion Private Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public RegistrationRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public RegistrationRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public async Task<Response<ContactDemographicsViewModel>> GetContactDemographics(long contactID)
        {
            string apiUrl = string.Format("{0}{1}", baseRoute, getContactDemographics);
            var parameters = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = await _communicationManager.GetAsync<Response<ContactDemographicsModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactAddressViewModel> GetContactAddress(long contactID)
        {
            string apiUrl = string.Format("{0}{1}", baseRoute, getContactAddress);
            var parameters = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Get<Response<ContactAddressModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }


        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactDemographicsViewModel> AddContactDemographics(ContactDemographicsViewModel contact)
        {
            string apiUrl = string.Format("{0}{1}", baseRoute, addContactDemographics);
            var response = _communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(contact.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactDemographicsViewModel> UpdateContactDemographics(ContactDemographicsViewModel contact)
        {
            string apiUrl = string.Format("{0}{1}", baseRoute, updateContactDemographics);
            var response = _communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(contact.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// DeleteContactDemographics to delete contact data
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="modifiedOn"></param>
        public void DeleteContactDemographics(long contactID, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"contactID", contactID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            _communicationManager.Delete(param, apiUrl);
        }

        /// <summary>
        /// Verifies the duplicate contacts.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactDemographicsViewModel> VerifyDuplicateContacts(ContactDemographicsViewModel contact)
        {
            string apiUrl = string.Format("{0}{1}", baseRoute, verifyDuplicateContacts);
            var response = _communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(contact.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the SSN.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<String> GetSSN(long contactID)
        {
            string apiUrl = string.Format("{0}{1}", baseRoute, "GetSSN");
            var parameters = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<String>>(parameters, apiUrl);

        }

        #endregion Public Methods
    }
}