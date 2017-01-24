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
    /// Repository for contact Race to call web api methods.
    /// </summary>
    /// <seealso cref="Axis.Plugins.Registration.IContactRaceRepository" />
    public class ContactRaceRepository : IContactRaceRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ContactRace/";

        /// <summary>
        /// constructor
        /// </summary>
        public ContactRaceRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="token">token</param>
        public ContactRaceRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact Race.
        /// </summary>
        /// <param name="contactID">Contact Id of contact</param>
        /// <returns>
        /// Response of type ContactEmailModel
        /// </returns>
        public Response<ContactRaceViewModel> GetContactRace(long contactID)
        {
            const string apiUrl = baseRoute + "GetContactRace";
            var param = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var retVal = communicationManager.Get<Response<ContactRaceModel>>(param, apiUrl);
            return retVal.ToViewModel();
        }

        /// <summary>
        /// Adds the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        public Response<ContactRaceViewModel> AddContactRace(List<ContactRaceViewModel> contactRace)
        {
            const string apiUrl = baseRoute + "AddContactRace";

            var response = communicationManager.Post<List<ContactRaceModel>, Response<ContactRaceModel>>(contactRace.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        public Response<ContactRaceViewModel> UpdateContactRace(List<ContactRaceViewModel> contactRace)
        {
            const string apiUrl = baseRoute + "UpdateContactRace";

            var response = communicationManager.Put<List<ContactRaceModel>, Response<ContactRaceModel>>(contactRace.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the contact Race.
        /// </summary>
        /// <param name="contactRaceID">The contact Race identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<ContactRaceViewModel> DeleteContactRace(long contactRaceID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteContactRace";
            var requestId = new NameValueCollection { { "contactRaceID", contactRaceID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<ContactRaceModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}