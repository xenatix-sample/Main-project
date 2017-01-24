using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    /// Registration repository to calling rule engine api
    /// </summary>
    public interface IRegistrationRepository
    {
        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Task<Response<ContactDemographicsViewModel>> GetContactDemographics(long contactID);

        /// <summary>
        /// Gets the Contact Address.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactAddressViewModel> GetContactAddress(long contactID);

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<ContactDemographicsViewModel> AddContactDemographics(ContactDemographicsViewModel contact);

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<ContactDemographicsViewModel> UpdateContactDemographics(ContactDemographicsViewModel contact);

        /// <summary>
        /// Deletes the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        void DeleteContactDemographics(long contactID, DateTime modifiedOn);

        /// <summary>
        /// Verifies the duplicate contacts.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<ContactDemographicsViewModel> VerifyDuplicateContacts(ContactDemographicsViewModel contact);

        /// <summary>
        /// Gets the SSN.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<String> GetSSN(long contactID);
    }
}