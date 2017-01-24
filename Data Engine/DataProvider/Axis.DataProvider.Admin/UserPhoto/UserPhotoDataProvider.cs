using Axis.Data.Repository;
using Axis.Model.Admin;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Admin.UserPhoto
{
    /// <summary>
    ///
    /// </summary>
    public class UserPhotoDataProvider : IUserPhotoDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPhotoDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public UserPhotoDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> GetUserPhoto(int userID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("UserID", userID) };
            var repository = _unitOfWork.GetRepository<UserPhotoModel>();
            var results = repository.ExecuteStoredProc("usp_GetUserPhoto", spParameters);

            return results;
        }

        /// <summary>
        /// Gets the user photo by identifier.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> GetUserPhotoById(long userPhotoID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("UserPhotoID", userPhotoID) };
            var repository = _unitOfWork.GetRepository<UserPhotoModel>();
            var results = repository.ExecuteStoredProc("usp_GetUserPhotoById", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> AddUserPhoto(UserPhotoModel userPhoto)
        {
            var spParameters = BuildUserPhotoSpParams(userPhoto, false);
            var repository = _unitOfWork.GetRepository<UserPhotoModel>();
            return _unitOfWork.EnsureInTransaction(
                    repository.ExecuteNQStoredProc,
                    "usp_AddUserPhoto",
                    spParameters,
                    forceRollback: userPhoto.ForceRollback.GetValueOrDefault(false),
                    idResult: true
                );
        }

        /// <summary>
        /// Updates the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> UpdateUserPhoto(UserPhotoModel userPhoto)
        {
            var spParameters = BuildUserPhotoSpParams(userPhoto, true);
            var repository = _unitOfWork.GetRepository<UserPhotoModel>();
            return _unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_UpdateUserPhoto", spParameters,
                          forceRollback: userPhoto.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes the user photo.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<UserPhotoModel> DeleteUserPhoto(long userPhotoID, DateTime modifiedOn)
        {
            var repository = _unitOfWork.GetRepository<UserPhotoModel>();
            var spParameters = new List<SqlParameter> { new SqlParameter("UserPhotoID", userPhotoID), new SqlParameter("ModifiedOn", modifiedOn) };
            var result = repository.ExecuteNQStoredProc("usp_DeleteUserPhoto", spParameters);
            return result;
        }

        #endregion exposed functionality

        #region Helpers

        /// <summary>
        /// Builds the user photo sp parameters.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        private List<SqlParameter> BuildUserPhotoSpParams(UserPhotoModel userPhoto, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();

            if (isUpdate)
                spParameters.Add(new SqlParameter("UserPhotoID", userPhoto.UserPhotoID));

            spParameters.Add(new SqlParameter("UserID", (object)userPhoto.UserID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("PhotoID", (object)userPhoto.PhotoID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsPrimary", (object)userPhoto.IsPrimary ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", userPhoto.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Helpers
    }
}
