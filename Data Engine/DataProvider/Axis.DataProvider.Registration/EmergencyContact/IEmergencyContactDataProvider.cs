using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// interface for emergency contact
    /// </summary>
    public interface IEmergencyContactDataProvider
    {
        Response<EmergencyContactModel> GetEmergencyContacts(long contactID, int contactTypeID);
        Response<EmergencyContactModel> AddEmergencyContact(EmergencyContactModel emergencyContactModel);
        Response<EmergencyContactModel> UpdateEmergencyContact(EmergencyContactModel emergencyContactModel);
        Response<EmergencyContactModel> DeleteEmergencyContact(long Id, DateTime modifiedOn);
    }
}