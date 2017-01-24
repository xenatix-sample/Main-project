using System;
using System.Web.Http;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Helpers.Controllers;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for Emergency contact
    /// </summary>
    public class EmergencyContactController : BaseApiController
    {
        readonly IEmergencyContactDataProvider emergencyContactDataProvider;

        public EmergencyContactController(IEmergencyContactDataProvider emergencyContactDataProvider)
        {
            this.emergencyContactDataProvider = emergencyContactDataProvider;
        }

        /// <summary>
        /// To get Emergency Contact list
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <param name="contactTypeId">Type of Contact</param>
        /// <returns></returns>
        public IHttpActionResult GetEmergencyContacts(long contactID, int contactTypeID)
        {
            return new HttpResult<Response<EmergencyContactModel>>(emergencyContactDataProvider.GetEmergencyContacts(contactID, contactTypeID), Request);
        }

        /// <summary>
        /// To add emergency contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency Contact Model</param>
        /// <returns></returns>
        public IHttpActionResult AddEmergencyContact(EmergencyContactModel emergencyContactModel)
        {
            return new HttpResult<Response<EmergencyContactModel>>(emergencyContactDataProvider.AddEmergencyContact(emergencyContactModel), Request);
        }

        /// <summary>
        /// To update emergency contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency Contact Model</param>
        /// <returns>Response of type EmergencyContactModel</returns>
        [HttpPut]
        public IHttpActionResult UpdateEmergencyContact(EmergencyContactModel emergencyContactModel)
        {
            return new HttpResult<Response<EmergencyContactModel>>(emergencyContactDataProvider.UpdateEmergencyContact(emergencyContactModel), Request);
        }

        /// <summary>
        /// To remove emergency contact
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteEmergencyContact(long Id, DateTime modifiedOn)
        {
            return new HttpResult<Response<EmergencyContactModel>>(emergencyContactDataProvider.DeleteEmergencyContact(Id, modifiedOn), Request);
        }

    }
}