using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    /// Rule engine class for calling service method of Emergency Contact Service
    /// </summary>
    public class EmergencyContactRuleEngine : IEmergencyContactRuleEngine
    {
        private readonly IEmergencyContactService emergencyContactService;

        public EmergencyContactRuleEngine(IEmergencyContactService emergencyContactService)
        {
            this.emergencyContactService = emergencyContactService;
        }

        /// <summary>
        /// To get Emergency Contacts of contact Id
        /// </summary>
        /// <param name="contactID">Contact Id of patient</param>
        /// <param name="contactTypeId">Contact Type Id for Emergency</param>
        /// <returns>Response type Emergency Contact Model</returns>
        public Response<EmergencyContactModel> GetEmergencyContacts(long contactID, int contactTypeId)
        {
            return emergencyContactService.GetEmergencyContacts(contactID, contactTypeId);
        }

        /// <summary>
        /// To add Emergency Contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency Contact Model</param>
        /// <returns>Response tpe Emergency Contact Model</returns>
        public Response<EmergencyContactModel> AddEmergencyContact(EmergencyContactModel emergencyContactModel)
        {
            return emergencyContactService.AddEmergencyContact(emergencyContactModel);
        }

        /// <summary>
        /// To update existing Emergency Contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency Contact Model</param>
        /// <returns>Response tpe Emergency Contact Model</returns>
        public Response<EmergencyContactModel> UpdateEmergencyContact(EmergencyContactModel emergencyContactModel)
        {
            return emergencyContactService.UpdateEmergencyContact(emergencyContactModel);
        }

        /// <summary>
        /// Remove Emergency Contact
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<EmergencyContactModel> DeleteEmergencyContact(long Id, DateTime modifiedOn)
        {
            return emergencyContactService.DeleteEmergencyContact(Id, modifiedOn);
        }
    }
}
