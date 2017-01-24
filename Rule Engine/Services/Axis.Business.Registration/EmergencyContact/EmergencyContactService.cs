using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration
{
    /// <summary>
    /// Emergency Contact Service class to call the web api methods
    /// </summary>
    public class EmergencyContactService : IEmergencyContactService
    {
        private readonly CommunicationManager communicationManager;
        private const string BaseRoute = "emergencyContact/";

        /// <summary>
        /// Constructor
        /// </summary>
        public EmergencyContactService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        public EmergencyContactService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// To get Emergency Contact list
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <param name="contactTypeId">Type of Contact</param>
        /// <returns>Response of type EmergencyContactModel</returns>
        public Response<EmergencyContactModel> GetEmergencyContacts(long contactID, int contactTypeId)
        {
            const string apiUrl = BaseRoute + "GetEmergencyContacts";
            var requestId = new NameValueCollection { 
                            { "contactID", contactID.ToString(CultureInfo.InvariantCulture) },
                            { "contactTypeId", contactTypeId.ToString(CultureInfo.InvariantCulture) } 
                            };
            return communicationManager.Get<Response<EmergencyContactModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// To add emergency contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency Contact Model</param>
        /// <returns>Response of type EmergencyContactModel</returns>
        public Response<EmergencyContactModel> AddEmergencyContact(EmergencyContactModel emergencyContactModel)
        {
            const string apiUrl = BaseRoute + "AddEmergencyContact";
            return communicationManager.Post<EmergencyContactModel, Response<EmergencyContactModel>>(emergencyContactModel, apiUrl);
        }

        /// <summary>
        /// To update emergency contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency Contact Model</param>
        /// <returns>Response of type EmergencyContactModel</returns>
        public Response<EmergencyContactModel> UpdateEmergencyContact(EmergencyContactModel emergencyContactModel)
        {
            const string apiUrl = BaseRoute + "UpdateEmergencyContact";
            return communicationManager.Put<EmergencyContactModel, Response<EmergencyContactModel>>(emergencyContactModel, apiUrl);
        }

        /// <summary>
        /// To remove emergency contact
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<EmergencyContactModel> DeleteEmergencyContact(long Id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteEmergencyContact";
            var requestId = new NameValueCollection
            {
                { "Id", Id.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<EmergencyContactModel>>(requestId, apiUrl);
        }

    }
}
