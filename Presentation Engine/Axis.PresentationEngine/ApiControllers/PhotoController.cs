using System;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Helpers.Model;
using Axis.PresentationEngine.Repository;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.PresentationEngine.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class PhotoController : BaseApiController
    {
        /// <summary>
        /// The photo repository
        /// </summary>
        private readonly IPhotoRepository photoRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoController"/> class.
        /// </summary>
        /// <param name="photoRepository">The photo repository.</param>
        public PhotoController(IPhotoRepository photoRepository)
        {
            this.photoRepository = photoRepository;
        }

        /// <summary>
        /// Gets the photo.
        /// </summary>
        /// <param name="photoID">The photo identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<PhotoViewModel> GetPhoto(long photoID)
        {
            return photoRepository.GetPhoto(photoID);
        }

        /// <summary>
        /// Adds the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PhotoViewModel> AddPhoto(PhotoViewModel photo)
        {
            return photoRepository.AddPhoto(photo);
        }

        /// <summary>
        /// Updates the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<PhotoViewModel> UpdatePhoto(PhotoViewModel photo)
        {
            return photoRepository.UpdatePhoto(photo);
        }

        /// <summary>
        /// Deletes the photo.
        /// </summary>
        /// <param name="photoID">The photo identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<PhotoViewModel> DeletePhoto(long photoID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return photoRepository.DeletePhoto(photoID, modifiedOn);
        }
    }
}