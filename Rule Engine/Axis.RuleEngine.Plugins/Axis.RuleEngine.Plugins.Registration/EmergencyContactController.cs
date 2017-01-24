using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Web.Http;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for emergency contact
    /// </summary>
    
    public class EmergencyContactController : BaseApiController
    {
        readonly IEmergencyContactRuleEngine emergencyContactRuleEngine;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="emergencyContactRuleEngine"></param>
        public EmergencyContactController(IEmergencyContactRuleEngine emergencyContactRuleEngine)
        {
            this.emergencyContactRuleEngine = emergencyContactRuleEngine;
        }

        /// <summary>
        /// Get emergency contacts for contact
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <param name="contactTypeId">Contact Type Id</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Read)]
        public IHttpActionResult GetEmergencyContacts(long contactID, int contactTypeId)
        {
            return new HttpResult<Response<EmergencyContactModel>>(emergencyContactRuleEngine.GetEmergencyContacts(contactID, contactTypeId), Request);
        }

        /// <summary>
        /// Add emergency contact for contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency contact model</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Create)]
        public IHttpActionResult AddEmergencyContact(EmergencyContactModel emergencyContactModel)
        {
            return new HttpResult<Response<EmergencyContactModel>>(emergencyContactRuleEngine.AddEmergencyContact(emergencyContactModel), Request);
        }

        /// <summary>
        /// Update emergency contact for model
        /// </summary>
        /// <param name="emergencyContactModel">Emergency contact model</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateEmergencyContact(EmergencyContactModel emergencyContactModel)
        {
            return new HttpResult<Response<EmergencyContactModel>>(emergencyContactRuleEngine.UpdateEmergencyContact(emergencyContactModel), Request);
        }

        /// <summary>
        /// Delete emergency contact
        /// </summary>
        /// <param name="Id">Contact Id of emergency contact</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteEmergencyContact(long Id, DateTime modifiedOn)
        {
            return new HttpResult<Response<EmergencyContactModel>>(emergencyContactRuleEngine.DeleteEmergencyContact(Id, modifiedOn), Request);
        }
    }
}
