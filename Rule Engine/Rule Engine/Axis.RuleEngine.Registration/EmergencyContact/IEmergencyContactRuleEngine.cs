using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    /// interface for Emergency Contact Rule Engine
    /// </summary>
    public interface IEmergencyContactRuleEngine
    {
        Response<EmergencyContactModel> GetEmergencyContacts(long contactID, int contactTypeId);
        Response<EmergencyContactModel> AddEmergencyContact(EmergencyContactModel emergencyContactModel);
        Response<EmergencyContactModel> UpdateEmergencyContact(EmergencyContactModel emergencyContactModel);
        Response<EmergencyContactModel> DeleteEmergencyContact(long Id, DateTime modifiedOn);
    }
}
