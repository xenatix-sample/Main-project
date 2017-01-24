using System;
using Axis.Model.Common;
using Axis.Model.Photo;

namespace Axis.Service.Common.Photo
{
    public interface IPhotoService
    {
        /// <summary>
        /// Gets the photo.
        /// </summary>
        /// <param name="PhotoID">The photo identifier.</param>
        /// <returns></returns>
        Response<PhotoModel> GetPhoto(long photoID);

        /// <summary>
        /// Adds the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        Response<PhotoModel> AddPhoto(PhotoModel photo);

        /// <summary>
        /// Updates the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        Response<PhotoModel> UpdatePhoto(PhotoModel photo);

        /// <summary>
        /// Deletes the photo.
        /// </summary>
        /// <param name="PhotoID">The photo identifier.</param>
        /// <returns></returns>
        Response<PhotoModel> DeletePhoto(long photoID, DateTime modifiedOn);
    }
}