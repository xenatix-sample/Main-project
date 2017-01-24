using Axis.Model.Common;
using Axis.Model.Registration;
using System;

namespace Axis.Service.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactDemographicsModel> GetContactDemographics(long contactID);

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<ContactDemographicsModel> AddContactDemographics(ContactDemographicsModel contact);

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<ContactDemographicsModel> UpdateContactDemographics(ContactDemographicsModel contact);

        /// <summary>
        /// Gets the client summary.
        /// </summary>
        /// <param name="SearchCriteria">The search criteria.</param>
        /// /// <param name="contactType">contact type of contact</param>
        /// <returns></returns>
        Response<ContactDemographicsModel> GetClientSummary(string SearchCriteria, string ContactType);
     
        /// <summary>
        /// Gets the Contact Address.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactAddressModel> GetContactAddress(long contactID);

        /// <summary>
        /// Verifies the duplicate contacts.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<ContactDemographicsModel> VerifyDuplicateContacts(ContactDemographicsModel contact);

        /// <summary>
        /// Gets the SSN.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<String> GetSSN(long contact);
    }
}