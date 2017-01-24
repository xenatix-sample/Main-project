using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using Axis.PresentationEngine.Areas.Admin.Translator;
using Axis.Service;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;

namespace Axis.PresentationEngine.Areas.Admin.Respository.UserPhoto
{
    /// <summary>
    ///
    /// </summary>
    public class UserPhotoRepository : IUserPhotoRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "userPhoto/";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPhotoRepository"/> class.
        /// </summary>
        public UserPhotoRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPhotoRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public UserPhotoRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public Response<UserPhotoViewModel> GetUserPhoto(int userID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfilePhoto" : "GetUserPhoto";
            string apiUrl = baseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString(CultureInfo.InvariantCulture) } };
            var userPhotos = communicationManager.Get<Response<UserPhotoModel>>(param, apiUrl);
            return userPhotos.ToViewModel();
        }

        /// <summary>
        /// Gets the user photo async
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<Response<UserPhotoViewModel>> GetUserPhotoAsync(int userID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfilePhoto" : "GetUserPhoto";
            string apiUrl = baseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString(CultureInfo.InvariantCulture) } };
            var userPhotos = await communicationManager.GetAsync<Response<UserPhotoModel>>(param, apiUrl);

            return userPhotos.ToViewModel();
        }

        /// <summary>
        /// Gets the user photo by identifier.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <returns></returns>
        public Response<UserPhotoViewModel> GetUserPhotoById(long userPhotoID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfilePhotoById" : "GetUserPhotoById";
            string apiUrl = baseRoute + route;
            var param = new NameValueCollection { { "userPhotoID", userPhotoID.ToString(CultureInfo.InvariantCulture) } };
            var userPhotos = communicationManager.Get<Response<UserPhotoModel>>(param, apiUrl);
            return userPhotos.ToViewModel();
        }

        /// <summary>
        /// Adds the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        public Response<UserPhotoViewModel> AddUserPhoto(UserPhotoViewModel userPhoto, bool isMyProfile)
        {
            var route = isMyProfile ? "AddMyProfilePhoto" : "AddUserPhoto";
            string apiUrl = baseRoute + route;
            var response = communicationManager.Post<UserPhotoModel, Response<UserPhotoModel>>(userPhoto.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        public Response<UserPhotoViewModel> UpdateUserPhoto(UserPhotoViewModel userPhoto, bool isMyProfile)
        {
            var route = isMyProfile ? "UpdateMyProfilePhoto" : "UpdateUserPhoto";
            string apiUrl = baseRoute + route;
            var response = communicationManager.Put<UserPhotoModel, Response<UserPhotoModel>>(userPhoto.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the user photo.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<UserPhotoViewModel> DeleteUserPhoto(long userPhotoID, DateTime modifiedOn, bool isMyProfile)
        {
            var route = isMyProfile ? "DeleteMyProfilePhoto" : "DeleteUserPhoto";
             string apiUrl = baseRoute + route;
            var requestId = new NameValueCollection { { "userPhotoID", userPhotoID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<UserPhotoModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}