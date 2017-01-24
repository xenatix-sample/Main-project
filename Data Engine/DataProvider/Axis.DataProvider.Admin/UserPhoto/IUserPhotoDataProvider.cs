using Axis.Model.Admin;
using Axis.Model.Common;
using System;

namespace Axis.DataProvider.Admin.UserPhoto
{
    /// <summary>
    ///
    /// </summary>
    public interface IUserPhotoDataProvider
    {
        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        Response<UserPhotoModel> GetUserPhoto(int userID);

        /// <summary>
        /// Gets the user photo by identifier.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <returns></returns>
        Response<UserPhotoModel> GetUserPhotoById(long userPhotoID);

        /// <summary>
        /// Adds the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        Response<UserPhotoModel> AddUserPhoto(UserPhotoModel userPhoto);

        /// <summary>
        /// Updates the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        Response<UserPhotoModel> UpdateUserPhoto(UserPhotoModel userPhoto);

        /// <summary>
        /// Deletes the user photo.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<UserPhotoModel> DeleteUserPhoto(long userPhotoID, DateTime modifiedOn);
    }
}