using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using Axis.PresentationEngine.Areas.Admin.Respository.UserPhoto;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Web.Http;

namespace Axis.PresentationEngine.Areas.Admin.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class UserPhotoController : BaseApiController
    {
        #region Private Variable

        /// <summary>
        /// The _user photo repository
        /// </summary>
        private readonly IUserPhotoRepository _userPhotoRepository;

        #endregion Private Variable

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPhotoController"/> class.
        /// </summary>
        /// <param name="userPhotoRepository">The user photo repository.</param>
        public UserPhotoController(IUserPhotoRepository userPhotoRepository)
        {
            this._userPhotoRepository = userPhotoRepository;
        }

        #endregion Contructors

        #region Data API

        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<UserPhotoViewModel> GetUserPhoto(int userID, bool isMyProfile)
        {
            return _userPhotoRepository.GetUserPhoto(userID, isMyProfile);
        }

        /// <summary>
        /// Gets the user photo by identifier.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<UserPhotoViewModel> GetUserPhotoById(long userPhotoID, bool isMyProfile)
        {
            return _userPhotoRepository.GetUserPhotoById(userPhotoID, isMyProfile);
        }

        /// <summary>
        /// Adds the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<UserPhotoViewModel> AddUserPhoto(UserPhotoViewModel userPhoto, bool isMyProfile)
        {
            return _userPhotoRepository.AddUserPhoto(userPhoto, isMyProfile);
        }

        /// <summary>
        /// Updates the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<UserPhotoViewModel> UpdateUserPhoto(UserPhotoViewModel userPhoto, bool isMyProfile)
        {
            return _userPhotoRepository.UpdateUserPhoto(userPhoto, isMyProfile);
        }

        /// <summary>
        /// Deletes the user photo.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<UserPhotoViewModel> DeleteUserPhoto(long userPhotoID, DateTime modifiedOn, bool isMyProfile)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _userPhotoRepository.DeleteUserPhoto(userPhotoID, modifiedOn, isMyProfile);
        }

        #endregion Data API
    }
}