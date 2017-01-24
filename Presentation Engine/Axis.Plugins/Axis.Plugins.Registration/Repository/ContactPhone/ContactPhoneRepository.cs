using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhoneRepository : IContactPhoneRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "contactPhones/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhoneRepository" /> class.
        /// </summary>
        public ContactPhoneRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhoneRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ContactPhoneRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact phones.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ContactPhoneModel> GetContactPhones(long contactID, int? contactTypeID)
        {
            const string apiUrl = baseRoute + "GetContactPhones";
            var requestParam = new NameValueCollection();
            requestParam.Add("contactID", contactID.ToString(CultureInfo.InvariantCulture));
            if (contactTypeID.HasValue)
            {
                requestParam.Add("contactTypeID", contactTypeID.Value.ToString(CultureInfo.InvariantCulture));
            }

            return communicationManager.Get<Response<ContactPhoneModel>>(requestParam, apiUrl);
        }

        /// <summary>
        /// Adds the update contact phones.
        /// </summary>
        /// <param name="contactPhoneModel">The contact phone model.</param>
        /// <returns></returns>
        public Response<ContactPhoneModel> AddUpdateContactPhones(ContactPhoneModel contactPhoneModel)
        {
            const string apiUrl = baseRoute + "AddUpdateContactPhones";
            var response = communicationManager.Post<ContactPhoneModel, Response<ContactPhoneModel>>(contactPhoneModel, apiUrl);
            return response;
        }

        /// <summary>
        /// Deletes the contact phones.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Response<ContactPhoneModel> DeleteContactPhones(long id, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteContactPhone";
            var requestId = new NameValueCollection { { "contactPhoneId", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<ContactPhoneModel>>(requestId, apiUrl);
            return response;
        }
    }
}