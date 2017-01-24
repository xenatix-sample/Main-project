using Axis.Configuration;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.Security;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Admin.UserPhoto
{
    /// <summary>
    ///
    /// </summary>
    public class UserPhotoService : IUserPhotoService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "userPhoto/";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPhotoService"/> class.
        /// </summary>
        public UserPhotoService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPhotoService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public UserPhotoService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> GetUserPhoto(int userID)
        {
            const string apiUrl = BaseRoute + "GetUserPhoto";
            var requestId = new NameValueCollection { { "userID", userID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<UserPhotoModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Gets the user photo by identifier.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> GetUserPhotoById(long userPhotoID)
        {
            const string apiUrl = BaseRoute + "GetUserPhotoById";
            var requestId = new NameValueCollection { { "userPhotoID", userPhotoID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<UserPhotoModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> AddUserPhoto(UserPhotoModel userPhoto)
        {
            const string apiUrl = BaseRoute + "AddUserPhoto";
            return communicationManager.Post<UserPhotoModel, Response<UserPhotoModel>>(userPhoto, apiUrl);
        }

        /// <summary>
        /// Updates the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> UpdateUserPhoto(UserPhotoModel userPhoto)
        {
            const string apiUrl = BaseRoute + "UpdateUserPhoto";
            return communicationManager.Put<UserPhotoModel, Response<UserPhotoModel>>(userPhoto, apiUrl);
        }

        /// <summary>
        /// Deletes the user photo.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> DeleteUserPhoto(long userPhotoID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteUserPhoto";
            var requestId = new NameValueCollection
            {
                { "userPhotoID", userPhotoID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<UserPhotoModel>>(requestId, apiUrl);
        }
    }
}