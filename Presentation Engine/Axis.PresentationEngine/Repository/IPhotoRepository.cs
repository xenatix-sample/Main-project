using System;
using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Repository
{
    public interface IPhotoRepository
    {
        /// <summary>
        /// Gets the photo.
        /// </summary>
        /// <param name="PhotoID">The photo identifier.</param>
        /// <returns></returns>
        Response<PhotoViewModel> GetPhoto(long photoID);

        /// <summary>
        /// Adds the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        Response<PhotoViewModel> AddPhoto(PhotoViewModel photo);

        /// <summary>
        /// Updates the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        Response<PhotoViewModel> UpdatePhoto(PhotoViewModel photo);

        /// <summary>
        /// Deletes the photo.
        /// </summary>
        /// <param name="PhotoID">The photo identifier.</param>
        /// <returns></returns>
        Response<PhotoViewModel> DeletePhoto(long photoID, DateTime modifiedOn);
    }
}