using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Photo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Common.Photo
{
    /// <summary>
    ///
    /// </summary>
    public class PhotoDataProvider : IPhotoDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PhotoDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the photo.
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public Response<PhotoModel> GetPhoto(long photoID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("PhotoID", photoID) };
            var repository = _unitOfWork.GetRepository<PhotoModel>(SchemaName.Core);
            var results = repository.ExecuteStoredProc("usp_GetPhoto", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        public Response<PhotoModel> AddPhoto(PhotoModel photo)
        {
            var spParameters = BuildPhotoSpParams(photo, false);
            var repository = _unitOfWork.GetRepository<PhotoModel>(SchemaName.Core);
            return _unitOfWork.EnsureInTransaction(
                    repository.ExecuteNQStoredProc,
                    "usp_AddPhoto",
                    spParameters,
                    forceRollback: photo.ForceRollback.GetValueOrDefault(false),
                    idResult: true
                );
        }

        /// <summary>
        /// Updates the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        public Response<PhotoModel> UpdatePhoto(PhotoModel photo)
        {
            var spParameters = BuildPhotoSpParams(photo, true);
            var repository = _unitOfWork.GetRepository<PhotoModel>(SchemaName.Core);
            return _unitOfWork.EnsureInTransaction(
                    repository.ExecuteNQStoredProc,
                    "usp_UpdatePhoto",
                    spParameters,
                    forceRollback: photo.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Deletes the photo.
        /// </summary>
        /// <param name="photoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<PhotoModel> DeletePhoto(long photoID, DateTime modifiedOn)
        {
            var repository = _unitOfWork.GetRepository<PhotoModel>(SchemaName.Core);
            var spParameters = new List<SqlParameter> { new SqlParameter("PhotoID", photoID), new SqlParameter("ModifiedOn", modifiedOn) };
            var result = repository.ExecuteNQStoredProc("usp_DeletePhoto", spParameters);
            return result;
        }

        #endregion exposed functionality

        #region Helpers

        /// <summary>
        /// Builds the photo sp parameters.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        private List<SqlParameter> BuildPhotoSpParams(PhotoModel photo, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();

            if (isUpdate)
                spParameters.Add(new SqlParameter("PhotoID", photo.PhotoID));

            SqlParameter param = new SqlParameter("PhotoBLOB", System.Data.SqlDbType.VarBinary, -1);
            param.SqlValue = (object)photo.PhotoBLOB ?? DBNull.Value;
            spParameters.Add(param);
            spParameters.Add(new SqlParameter("ThumbnailBLOB", (object)photo.ThumbnailBLOB ?? DBNull.Value));
            spParameters.Add(new SqlParameter("TakenBy", (object)photo.TakenBy ?? DBNull.Value));
            spParameters.Add(new SqlParameter("TakenTime", (object)photo.TakenTime ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", photo.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Helpers
    }
}
