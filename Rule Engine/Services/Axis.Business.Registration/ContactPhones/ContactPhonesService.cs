using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhonesService : IContactPhonesService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "contactPhones/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhonesService" /> class.
        /// </summary>
        public ContactPhonesService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the contact phones.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ContactPhoneModel> GetContactPhones(long contactID, int? contactTypeID)
        {
            const string apiUrl = BaseRoute + "GetContactPhones";
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
            const string apiUrl = BaseRoute + "AddUpdateContactPhones";
            return communicationManager.Post<ContactPhoneModel, Response<ContactPhoneModel>>(contactPhoneModel, apiUrl);
        }

        /// <summary>
        /// Deletes the contact phones.
        /// </summary>
        /// <param name="contactPhoneId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactPhoneModel> DeleteContactPhone(long contactPhoneId, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteContactPhone";
            var requestId = new NameValueCollection
            {
                { "contactPhoneId", contactPhoneId.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ContactPhoneModel>>(requestId, apiUrl);
        }
    }
}