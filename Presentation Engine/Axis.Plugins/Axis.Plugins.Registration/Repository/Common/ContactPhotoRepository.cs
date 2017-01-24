using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Registration.Repository.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhotoRepository : IContactPhotoRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "contactPhoto/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhotoRepository"/> class.
        /// </summary>
        public ContactPhotoRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhotoRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ContactPhotoRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact photo.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoViewModel> GetContactPhoto(long contactID)
        {
            const string apiUrl = baseRoute + "GetContactPhoto";
            var param = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var retVal = communicationManager.Get<Response<ContactPhotoModel>>(param, apiUrl);
            return retVal.ToViewModel();
        }

        /// <summary>
        /// Gets the contact photo by identifier.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoViewModel> GetContactPhotoById(long contactPhotoID)
        {
            const string apiUrl = baseRoute + "GetContactPhotoById";
            var param = new NameValueCollection { { "contactPhotoID", contactPhotoID.ToString(CultureInfo.InvariantCulture) } };
            var retVal = communicationManager.Get<Response<ContactPhotoModel>>(param, apiUrl);
            return retVal.ToViewModel();
        }

        /// <summary>
        /// Gets the contact photo thumbnails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoViewModel> GetContactPhotoThumbnails(long contactID)
        {
            const string apiUrl = baseRoute + "GetContactPhotoThumbnails";
            var param = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var retVal = communicationManager.Get<Response<ContactPhotoModel>>(param, apiUrl);
            return retVal.ToViewModel();
        }

        /// <summary>
        /// Adds the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        public Response<ContactPhotoViewModel> AddContactPhoto(ContactPhotoViewModel contactPhoto)
        {
            const string apiUrl = baseRoute + "AddContactPhoto";
            var response = communicationManager.Post<ContactPhotoModel, Response<ContactPhotoModel>>(contactPhoto.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        public Response<ContactPhotoViewModel> UpdateContactPhoto(ContactPhotoViewModel contactPhoto)
        {
            const string apiUrl = baseRoute + "UpdateContactPhoto";
            var response = communicationManager.Put<ContactPhotoModel, Response<ContactPhotoModel>>(contactPhoto.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoViewModel> DeleteContactPhoto(long contactPhotoID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteContactPhoto";
            var requestId = new NameValueCollection { { "contactPhotoID", contactPhotoID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<ContactPhotoModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}