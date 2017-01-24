using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using System;
using System.Threading.Tasks;

namespace Axis.PresentationEngine.Areas.Admin.Respository.UserPhoto
{
    public interface IUserPhotoRepository
    {
        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        Response<UserPhotoViewModel> GetUserPhoto(int userID, bool isMyProfile);

        /// <summary>
        /// Gets the user photo async
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<Response<UserPhotoViewModel>> GetUserPhotoAsync(int userID, bool isMyProfile);

        /// <summary>
        /// Gets the user photo by identifier.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <returns></returns>
        Response<UserPhotoViewModel> GetUserPhotoById(long userPhotoID, bool isMyProfile);

        /// <summary>
        /// Adds the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        Response<UserPhotoViewModel> AddUserPhoto(UserPhotoViewModel userPhoto, bool isMyProfile);

        /// <summary>
        /// Updates the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        Response<UserPhotoViewModel> UpdateUserPhoto(UserPhotoViewModel userPhoto, bool isMyProfile);

        /// <summary>
        /// Deletes the user photo.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<UserPhotoViewModel> DeleteUserPhoto(long userPhotoID, DateTime modifiedOn, bool isMyProfile);
    }
}