using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class RegistrationRuleEngine : IRegistrationRuleEngine
    {
        /// <summary>
        /// The _registration service
        /// </summary>
        private readonly IRegistrationService _registrationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationRuleEngine"/> class.
        /// </summary>
        /// <param name="registrationService">The registration service.</param>
        public RegistrationRuleEngine(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> GetContactDemographics(long contactID)
        {
            return _registrationService.GetContactDemographics(contactID);
        }

        /// <summary>
        /// Gets the Contact Address.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactAddressModel> GetContactAddress(long contactID)
        {
            return _registrationService.GetContactAddress(contactID);
        }

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> AddContactDemographics(ContactDemographicsModel contact)
        {
            return _registrationService.AddContactDemographics(contact);
        }

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> UpdateContactDemographics(ContactDemographicsModel contact)
        {
            return _registrationService.UpdateContactDemographics(contact);
        }

        /// <summary>
        /// Get the contact details based on search criteria
        /// </summary>
        /// <param name="searchCriteria">Text to Search</param>
        /// <param name="contactType">contact type of contact</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> GetClientSummary(string searchCriteria, string ContactType)
        {
            if (string.IsNullOrEmpty(searchCriteria) || string.IsNullOrWhiteSpace(searchCriteria))
            {
                searchCriteria = "";
            }
            return _registrationService.GetClientSummary(searchCriteria,ContactType);
        }

        /// <summary>
        /// Verifies the duplicate contacts.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> VerifyDuplicateContacts(ContactDemographicsModel contact)
        {
            return _registrationService.VerifyDuplicateContacts(contact);
        }

        /// <summary>
        /// Gets the SSN.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<String> GetSSN(long contactID)
        {
            return _registrationService.GetSSN(contactID);
        }
    }
}