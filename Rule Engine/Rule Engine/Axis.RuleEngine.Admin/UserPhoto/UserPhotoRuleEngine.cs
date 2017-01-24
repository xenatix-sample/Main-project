using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.Service.Admin.UserPhoto;
using System;

namespace Axis.RuleEngine.Admin.UserPhoto
{
    /// <summary>
    ///
    /// </summary>
    public class UserPhotoRuleEngine : IUserPhotoRuleEngine
    {
        /// <summary>
        /// The user photo service
        /// </summary>
        private readonly IUserPhotoService userPhotoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPhotoRuleEngine"/> class.
        /// </summary>
        /// <param name="userPhotoService">The user photo service.</param>
        public UserPhotoRuleEngine(IUserPhotoService userPhotoService)
        {
            this.userPhotoService = userPhotoService;
        }

        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> GetUserPhoto(int userID)
        {
            return userPhotoService.GetUserPhoto(userID);
        }

        /// <summary>
        /// Gets the user photo by identifier.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> GetUserPhotoById(long userPhotoID)
        {
            return userPhotoService.GetUserPhotoById(userPhotoID);
        }

        /// <summary>
        /// Adds the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> AddUserPhoto(UserPhotoModel userPhoto)
        {
            return userPhotoService.AddUserPhoto(userPhoto);
        }

        /// <summary>
        /// Updates the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> UpdateUserPhoto(UserPhotoModel userPhoto)
        {
            return userPhotoService.UpdateUserPhoto(userPhoto);
        }

        /// <summary>
        /// Deletes the user photo.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> DeleteUserPhoto(long userPhotoID, DateTime modifiedOn)
        {
            return userPhotoService.DeleteUserPhoto(userPhotoID, modifiedOn);
        }
    }
}