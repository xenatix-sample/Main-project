using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.Service.Registration
{
    /// <summary>
    /// Interface of Emergency Contact Service class to call the web api methods
    /// </summary>
    public interface IEmergencyContactService
    {
        Response<EmergencyContactModel> GetEmergencyContacts(long contactID, int contactTypeId);
        Response<EmergencyContactModel> AddEmergencyContact(EmergencyContactModel emergencyContactModel);
        Response<EmergencyContactModel> UpdateEmergencyContact(EmergencyContactModel emergencyContactModel);
        Response<EmergencyContactModel> DeleteEmergencyContact(long Id, DateTime modifiedOn);
    }
}
