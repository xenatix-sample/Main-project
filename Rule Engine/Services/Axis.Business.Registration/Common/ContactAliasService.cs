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
    /// Contact Alias Service class to call the web api methods
    /// </summary>
    /// <seealso cref="Axis.Service.Registration.IContactAliasService" />
    public class ContactAliasService : IContactAliasService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ContactAlias/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ContactAliasService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">The token.</param>
        public ContactAliasService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact alias.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactAliasModel> GetContactAlias(long contactID)
        {
            const string apiUrl = BaseRoute + "GetContactAlias";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactAliasModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        public Response<ContactAliasModel> AddContactAlias(List<ContactAliasModel> contactAlias)
        {
            const string apiUrl = BaseRoute + "AddContactAlias";
            return communicationManager.Post<List<ContactAliasModel>, Response<ContactAliasModel>>(contactAlias, apiUrl);
        }

        /// <summary>
        /// Updates the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        public Response<ContactAliasModel> UpdateContactAlias(List<ContactAliasModel> contactAlias)
        {
            const string apiUrl = BaseRoute + "UpdateContactAlias";
            return communicationManager.Put<List<ContactAliasModel>, Response<ContactAliasModel>>(contactAlias, apiUrl);
        }

        /// <summary>
        /// Deletes the contact alias.
        /// </summary>
        /// <param name="contactAliasID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<ContactAliasModel> DeleteContactAlias(long contactAliasID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteContactAlias";
            var requestId = new NameValueCollection
            {
                { "contactAliasID", contactAliasID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ContactAliasModel>>(requestId, apiUrl);
        }
    }
}