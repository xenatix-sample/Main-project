using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration
{
    /// <summary>
    /// Email Service class to call the web api methods
    /// </summary>
    public class ContactEmailService : IContactEmailService
    {
        private readonly CommunicationManager communicationManager;
        private const string BaseRoute = "ContactEmail/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ContactEmailService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public ContactEmailService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// To get the list of Email
        /// </summary>
        /// <param name="contactID">Contact Id</param>        
        /// <returns></returns>
        public Response<ContactEmailModel> GetEmails(long contactID, int contactTypeID)
        {
            const string apiUrl = BaseRoute + "GetEmails";
            var requestId = new NameValueCollection { 
                            { "contactID", contactID.ToString(CultureInfo.InvariantCulture) },{ "contactTypeID", contactTypeID.ToString(CultureInfo.InvariantCulture) }};
            return communicationManager.Get<Response<ContactEmailModel>>(requestId, apiUrl);
        }

       
        /// <summary>
        /// To add/update Email
        /// </summary>
        /// <param name="ContactEmailModel"></param>
        /// <returns></returns>
        public Response<ContactEmailModel> AddUpdateEmails(List<ContactEmailModel> contact)
        {
            const string apiUrl = BaseRoute + "AddUpdateEmails";
            return communicationManager.Post<List<ContactEmailModel>, Response<ContactEmailModel>>(contact, apiUrl);
        }

        /// <summary>
        /// To remove Email
        /// </summary>
        /// <param name="contactEmailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactEmailModel> DeleteEmail(long contactEmailID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteEmail";
            var requestId = new NameValueCollection
            {
                { "contactEmailID", contactEmailID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ContactEmailModel>>(requestId, apiUrl);
        }
    }
}
