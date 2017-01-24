
using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    /// Interface for Emergency Contact Repository to call web api methods.
    /// </summary>
    public interface IEmergencyContactRepository
    {
        Response<EmergencyContactModel> GetEmergencyContacts(long contactID, int contactTypeId);
        Task<Response<EmergencyContactModel>> GetEmergencyContactsAsync(long contactID, int contactTypeId);
        Response<EmergencyContactViewModel> AddEmergencyContact(EmergencyContactViewModel emergencyContact);
        Response<EmergencyContactViewModel> UpdateEmergencyContact(EmergencyContactViewModel emergencyContact);
        Response<EmergencyContactViewModel> DeleteEmergencyContact(long Id, DateTime modifiedOn);
    }
}
