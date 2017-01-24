using System;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// Controller for Emergency Controller screen
    /// </summary>
    public class EmergencyContactController : BaseApiController
    {
        private readonly IEmergencyContactRepository emergencyContactRepository;

        public EmergencyContactController(IEmergencyContactRepository emergencyContactRepository)
        {
            this.emergencyContactRepository = emergencyContactRepository;
        }

        /// <summary>
        /// To get Emergency contacts for contact
        /// </summary>
        /// <param name="contactID">Contact Id of patient</param>
        /// <param name="contactTypeID">Contact type Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<EmergencyContactModel>> GetEmergencyContacts(int contactID, int contactTypeID)
        {
            var result = await emergencyContactRepository.GetEmergencyContactsAsync(contactID, contactTypeID);
            return result;
        }

        /// <summary>
        /// Add emergency contact for contact
        /// </summary>
        /// <param name="emergencyContact">Emergency Contact ViewModel</param>
        /// <returns></returns>
        public Response<EmergencyContactViewModel> AddEmergencyContact(EmergencyContactViewModel emergencyContact)
        {
            return emergencyContactRepository.AddEmergencyContact(emergencyContact);
        }

        /// <summary>
        /// Update Emergency contact for contact
        /// </summary>
        /// <param name="emergencyContact">Emergency Contact ViewModel</param>
        /// <returns></returns>
        [HttpPut]
        public Response<EmergencyContactViewModel> UpdateEmergencyContact(EmergencyContactViewModel emergencyContact)
        {
            return emergencyContactRepository.UpdateEmergencyContact(emergencyContact);
        }

        /// <summary>
        /// Delete emergency contact for contact
        /// </summary>
        /// <param name="Id">Contact Id of emergency contact</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<EmergencyContactViewModel> DeleteEmergencyContact(long Id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return emergencyContactRepository.DeleteEmergencyContact(Id, modifiedOn);
        }
    }
}
