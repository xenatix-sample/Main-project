
using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Axis.Plugins.Registration.Translator;
using Axis.Model.Registration.Model;
using Axis.Model.Email;
using Axis.Plugins.Registration.Model;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    /// Repository for Email to call web api methods.
    /// </summary>
    public class ContactEmailRepository : IContactEmailRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "ContactEmail/";

        /// <summary>
        /// constructor
        /// </summary>
        public ContactEmailRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="token">token</param>
        public ContactEmailRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

     

        /// <summary>
        /// To get Email list for contact
        /// </summary>
        /// <param name="contactID">Contact Id of contact</param>        
        /// <returns>Response of type ContactEmailModel</returns>
        public Response<ContactEmailViewModel> GetEmails(long contactID, int contactTypeID)
        {
            const string apiUrl = baseRoute + "GetEmails";
            var param = new NameValueCollection { 
                            { "contactID", contactID.ToString(CultureInfo.InvariantCulture) },
                            { "contactTypeID", contactTypeID.ToString(CultureInfo.InvariantCulture) } };            
            var retVal = communicationManager.Get<Response<ContactEmailModel>>(param, apiUrl);
            return retVal.ToViewModel();
        }
       

        /// <summary>
        /// To update Email for contact
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Response<ContactEmailViewModel> AddUpdateEmail(List<ContactEmailViewModel> emails)
        {
            const string apiUrl = baseRoute + "AddUpdateEmail";
           
            var response = communicationManager.Post<List<ContactEmailModel>, Response<ContactEmailModel>>(emails.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// To remove Email for contact
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Response<ContactEmailViewModel> DeleteEmail(long contactEmailID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteEmail";
            var requestId = new NameValueCollection { { "contactEmailID", contactEmailID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<ContactEmailModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}