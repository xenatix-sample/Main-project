
using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Axis.Plugins.Registration.Translator;
using Axis.Constant;


namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    /// Repository for Emergency Contact to call web api methods.
    /// </summary>
    public class EmergencyContactRepository : IEmergencyContactRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "emergencyContact/";

        public EmergencyContactRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public EmergencyContactRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// To get Emergency Contact list for contact
        /// </summary>
        /// <param name="contactID">Contact Id of contact</param>
        /// <param name="contactTypeId">Type of Contact</param>
        /// <returns>Response of type EmergencyContactModel</returns>
        
        public Response<EmergencyContactModel> GetEmergencyContacts(long contactID, int contactTypeId)
        {
            return GetEmergencyContactsAsync(contactID, contactTypeId).Result;
        }

        /// <summary>
        /// To get Emergency Contact list for contact
        /// </summary>
        /// <param name="contactID">Contact Id of contact</param>
        /// <param name="contactTypeId">Type of Contact</param>
        /// <returns>Response of type EmergencyContactModel</returns>
      
        public async Task<Response<EmergencyContactModel>> GetEmergencyContactsAsync(long contactID, int contactTypeId)
        {
            const string apiUrl = baseRoute + "GetEmergencyContacts";
            var param = new NameValueCollection { 
                            { "contactID", contactID.ToString(CultureInfo.InvariantCulture) },
                            { "contactTypeId", contactTypeId.ToString(CultureInfo.InvariantCulture) } };
            return await communicationManager.GetAsync<Response<EmergencyContactModel>>(param, apiUrl);
        }

        /// <summary>
        /// To add emergency contact for contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency Contact Model</param>
        /// <returns>Response of type EmergencyContactViewModel</returns>
    
        public Response<EmergencyContactViewModel> AddEmergencyContact(EmergencyContactViewModel emergencyContact)
        {
            const string apiUrl = baseRoute + "AddEmergencyContact";
            var response = communicationManager.Post<EmergencyContactModel, Response<EmergencyContactModel>>(emergencyContact.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// To update emergency contact for contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency Contact Model</param>
        /// <returns>Response of type EmergencyContactViewModel</returns>
     
        public Response<EmergencyContactViewModel> UpdateEmergencyContact(EmergencyContactViewModel emergencyContact)
        {
            const string apiUrl = baseRoute + "UpdateEmergencyContact";
            var response = communicationManager.Put<EmergencyContactModel, Response<EmergencyContactModel>>(emergencyContact.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// To remove emergency contact for contact
        /// </summary>
        /// <param name="Id">Contact Id of emergency contact</param>
        /// <returns>Response of type EmergencyContactModel</returns>
      
        public Response<EmergencyContactViewModel> DeleteEmergencyContact(long Id, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteEmergencyContact";
            var requestId = new NameValueCollection { { "Id", Id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<EmergencyContactModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }

    }
}
