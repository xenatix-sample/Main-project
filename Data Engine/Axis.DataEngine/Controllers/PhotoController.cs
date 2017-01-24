using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Common.Photo;
using Axis.Model.Common;
using Axis.Model.Photo;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class PhotoController : BaseApiController
    {
        /// <summary>
        /// The photo data provider
        /// </summary>
        private readonly IPhotoDataProvider photoDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoController"/> class.
        /// </summary>
        /// <param name="photoDataProvider">The photo data provider.</param>
        public PhotoController(IPhotoDataProvider photoDataProvider)
        {
            this.photoDataProvider = photoDataProvider;
        }

        /// <summary>
        /// Gets the photo.
        /// </summary>
        /// <param name="photoID">The photo identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPhoto(long photoID)
        {
            return new HttpResult<Response<PhotoModel>>(photoDataProvider.GetPhoto(photoID), Request);
        }

        /// <summary>
        /// Adds the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPhoto(PhotoModel photo)
        {
            return new HttpResult<Response<PhotoModel>>(photoDataProvider.AddPhoto(photo), Request);
        }

        /// <summary>
        /// Updates the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdatePhoto(PhotoModel photo)
        {
            return new HttpResult<Response<PhotoModel>>(photoDataProvider.UpdatePhoto(photo), Request);
        }

        /// <summary>
        /// Deletes the photo.
        /// </summary>
        /// <param name="photoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeletePhoto(long photoID, DateTime modifiedOn)
        {
            return new HttpResult<Response<PhotoModel>>(photoDataProvider.DeletePhoto(photoID, modifiedOn), Request);
        }
    }
}