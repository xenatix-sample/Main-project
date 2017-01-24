using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhotoService : IContactPhotoService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "contactPhoto/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhotoService"/> class.
        /// </summary>
        public ContactPhotoService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhotoService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ContactPhotoService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact photo.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> GetContactPhoto(long contactID)
        {
            const string apiUrl = BaseRoute + "GetContactPhoto";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactPhotoModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Gets the contact photo by identifier.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> GetContactPhotoById(long contactPhotoID)
        {
            const string apiUrl = BaseRoute + "GetContactPhotoById";
            var requestId = new NameValueCollection { { "contactPhotoID", contactPhotoID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactPhotoModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Gets the contact photo thumbnails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> GetContactPhotoThumbnails(long contactID)
        {
            const string apiUrl = BaseRoute + "GetContactPhotoThumbnails";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactPhotoModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> AddContactPhoto(ContactPhotoModel contactPhoto)
        {
            const string apiUrl = BaseRoute + "AddContactPhoto";
            return communicationManager.Post<ContactPhotoModel, Response<ContactPhotoModel>>(contactPhoto, apiUrl);
        }

        /// <summary>
        /// Updates the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> UpdateContactPhoto(ContactPhotoModel contactPhoto)
        {
            const string apiUrl = BaseRoute + "UpdateContactPhoto";
            return communicationManager.Put<ContactPhotoModel, Response<ContactPhotoModel>>(contactPhoto, apiUrl);
        }

        /// <summary>
        /// Deletes the contact photo.
        /// </summary>
        /// <param name="contactPhotoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactPhotoModel> DeleteContactPhoto(long contactPhotoID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteContactPhoto";
            var requestId = new NameValueCollection
            {
                { "contactPhotoID", contactPhotoID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ContactPhotoModel>>(requestId, apiUrl);
        }
    }
}