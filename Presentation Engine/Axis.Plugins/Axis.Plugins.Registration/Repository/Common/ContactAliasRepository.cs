using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    /// Repository for contact alias to call web api methods.
    /// </summary>
    /// <seealso cref="Axis.Plugins.Registration.IContactAliasRepository" />
    public class ContactAliasRepository : IContactAliasRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ContactAlias/";

        /// <summary>
        /// constructor
        /// </summary>
        public ContactAliasRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="token">token</param>
        public ContactAliasRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact alias.
        /// </summary>
        /// <param name="contactID">Contact Id of contact</param>
        /// <returns>
        /// Response of type ContactEmailModel
        /// </returns>
        public Response<ContactAliasViewModel> GetContactAlias(long contactID)
        {
            const string apiUrl = baseRoute + "GetContactAlias";
            var param = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var retVal = communicationManager.Get<Response<ContactAliasModel>>(param, apiUrl);
            return retVal.ToViewModel();
        }

        /// <summary>
        /// Adds the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        public Response<ContactAliasViewModel> AddContactAlias(List<ContactAliasViewModel> contactAlias)
        {
            const string apiUrl = baseRoute + "AddContactAlias";

            var response = communicationManager.Post<List<ContactAliasModel>, Response<ContactAliasModel>>(contactAlias.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        public Response<ContactAliasViewModel> UpdateContactAlias(List<ContactAliasViewModel> contactAlias)
        {
            const string apiUrl = baseRoute + "UpdateContactAlias";

            var response = communicationManager.Put<List<ContactAliasModel>, Response<ContactAliasModel>>(contactAlias.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the contact alias.
        /// </summary>
        /// <param name="contactAliasID">The contact alias identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<ContactAliasViewModel> DeleteContactAlias(long contactAliasID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteContactAlias";
            var requestId = new NameValueCollection { { "contactAliasID", contactAliasID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<ContactAliasModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}