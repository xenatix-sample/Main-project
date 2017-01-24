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
    /// Contact Race Service class to call the web api methods
    /// </summary>
    /// <seealso cref="Axis.Service.Registration.IContactRaceService" />
    public class ContactRaceService : IContactRaceService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ContactRace/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ContactRaceService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">The token.</param>
        public ContactRaceService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact Race.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactRaceModel> GetContactRace(long contactID)
        {
            const string apiUrl = BaseRoute + "GetContactRace";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactRaceModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        public Response<ContactRaceModel> AddContactRace(List<ContactRaceModel> contactRace)
        {
            const string apiUrl = BaseRoute + "AddContactRace";
            return communicationManager.Post<List<ContactRaceModel>, Response<ContactRaceModel>>(contactRace, apiUrl);
        }

        /// <summary>
        /// Updates the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        public Response<ContactRaceModel> UpdateContactRace(List<ContactRaceModel> contactRace)
        {
            const string apiUrl = BaseRoute + "UpdateContactRace";
            return communicationManager.Put<List<ContactRaceModel>, Response<ContactRaceModel>>(contactRace, apiUrl);
        }

        /// <summary>
        /// Deletes the contact Race.
        /// </summary>
        /// <param name="contactRaceID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<ContactRaceModel> DeleteContactRace(long contactRaceID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteContactRace";
            var requestId = new NameValueCollection
            {
                { "contactRaceID", contactRaceID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ContactRaceModel>>(requestId, apiUrl);
        }
    }
}