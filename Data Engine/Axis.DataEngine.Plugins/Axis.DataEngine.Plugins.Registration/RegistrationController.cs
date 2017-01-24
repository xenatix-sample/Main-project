using System.Web.Http;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Helpers.Controllers;
using System;

namespace Axis.DataEngine.Plugins.Registration
{

    /// <summary>
    /// 
    /// </summary>
    public class RegistrationController : BaseApiController
    {
        /// <summary>
        /// The _registration data provider
        /// </summary>
        readonly IRegistrationDataProvider _registrationDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationController"/> class.
        /// </summary>
        /// <param name="registrationDataProvider">The registration data provider.</param>
        public RegistrationController(IRegistrationDataProvider registrationDataProvider)
        {
            _registrationDataProvider = registrationDataProvider;
        }

        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public IHttpActionResult GetContactDemographics(long contactID)
        {
            return new HttpResult<Response<ContactDemographicsModel>>(_registrationDataProvider.GetContactDemographics(contactID), Request);
        }

        /// <summary>
        /// Gets the Contact Address.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public IHttpActionResult GetContactAddress(long contactID)
        {
            return new HttpResult<Response<ContactAddressModel>>(_registrationDataProvider.GetContactAddress(contactID), Request);
        }

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public IHttpActionResult AddContactDemographics(ContactDemographicsModel contact)
        {
            return new HttpResult<Response<ContactDemographicsModel>>(_registrationDataProvider.AddContactDemographics(contact), Request);
        }

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public IHttpActionResult UpdateContactDemographics(ContactDemographicsModel contact)
        {
            return new HttpResult<Response<ContactDemographicsModel>>(_registrationDataProvider.UpdateContactDemographics(contact), Request);
        }

        /// <summary>
        /// Get client records based on the search criteria
        /// </summary>
        /// <param name="SearchCriteria">Search Criteria entered by user</param>
        /// <param name="contactType">contact type of contact</param>
        /// <returns>
        /// ContactDemographicsModel
        /// </returns>
        public IHttpActionResult GetClientSummary(string SearchCriteria, string ContactType)
        {
            if (string.IsNullOrEmpty(SearchCriteria) || string.IsNullOrWhiteSpace(SearchCriteria))
            {
                SearchCriteria = "";
            }
            return new HttpResult<Response<ContactDemographicsModel>>(_registrationDataProvider.GetClientSummary(SearchCriteria,ContactType), Request);
        }

        /// <summary>
        /// Verifies the duplicate contacts.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VerifyDuplicateContacts(ContactDemographicsModel contact)
        {
            return new HttpResult<Response<ContactDemographicsModel>>(_registrationDataProvider.VerifyDuplicateContacts(contact), Request);
        }

        /// <summary>
        /// Gets the SSN.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public IHttpActionResult GetSSN(long contactID)
        {
            return new HttpResult<Response<String>>(_registrationDataProvider.GetSSN(contactID), Request);
        }
    }
}