using System;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class RegistrationController : BaseApiController
    {
        /// <summary>
        /// The registration repository
        /// </summary>
        private readonly IRegistrationRepository registrationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationController"/> class.
        /// </summary>
        /// <param name="registrationRepository">The registration repository.</param>
        public RegistrationController(IRegistrationRepository registrationRepository)
        {
            this.registrationRepository = registrationRepository;
        }

        #region Client Demography

        /// <summary>
        /// GetContactDemographics to get data on basis of contactID
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns>response</returns>
        [HttpGet]
        public async Task<Response<ContactDemographicsViewModel>> GetContactDemographics(long contactID)
        {
            var model = await registrationRepository.GetContactDemographics(contactID);
            return model;

        }

        /// <summary>
        /// AddContactDemographics method to save the contact data 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>response</returns>
        [HttpPost]
        public Response<ContactDemographicsViewModel> AddContactDemographics(ContactDemographicsViewModel contact)
        {

            var response = registrationRepository.AddContactDemographics(contact);
            return response;
        }

        /// <summary>
        /// UpdateContactDemographics method to update the contact data
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>response</returns>
        [HttpPut]
        public Response<ContactDemographicsViewModel> UpdateContactDemographics(ContactDemographicsViewModel contact)
        {
            var response = registrationRepository.UpdateContactDemographics(contact);
            return response;
        }

        /// <summary>
        /// Delete Method to delete the contact data
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns>response</returns>
        [HttpDelete]
        public void DeleteContactDemographics(long ContactID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            registrationRepository.DeleteContactDemographics(ContactID, modifiedOn);
        }

        /// <summary>
        /// Get Contact Address on basis of contactID
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns>response</returns>
        [HttpGet]
        public Response<ContactAddressViewModel> GetContactAddress(long contactID)
        {
            var result = registrationRepository.GetContactAddress(contactID);
            return result;
        }

        /// <summary>
        /// Verifies the duplicate contacts.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ContactDemographicsViewModel> VerifyDuplicateContacts(ContactDemographicsViewModel contact)
        {
            var result = registrationRepository.VerifyDuplicateContacts(contact);
            return result;
        }

        /// <summary>
        /// Gets the SSN.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<String> GetSSN(long contactID)
        {
           return registrationRepository.GetSSN(contactID);
        }


        #endregion
    }
}