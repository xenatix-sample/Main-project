using System;
using Axis.Model.Common;
using Axis.Model.Photo;
using Axis.RuleEngine.Common.Photo;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class PhotoController : BaseApiController
    {
        /// <summary>
        /// The photo rule engine
        /// </summary>
        private readonly IPhotoRuleEngine photoRuleEngine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoController"/> class.
        /// </summary>
        /// <param name="photoRuleEngine">The photo rule engine.</param>
        public PhotoController(IPhotoRuleEngine photoRuleEngine)
        {
            this.photoRuleEngine = photoRuleEngine;
        }

        /// <summary>
        /// Gets the photo.
        /// </summary>
        /// <param name="photoID">The photo identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPhoto(long photoID)
        {
            return new HttpResult<Response<PhotoModel>>(photoRuleEngine.GetPhoto(photoID), Request);
        }

        /// <summary>
        /// Adds the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPhoto(PhotoModel photo)
        {
            return new HttpResult<Response<PhotoModel>>(photoRuleEngine.AddPhoto(photo), Request);
        }

        /// <summary>
        /// Updates the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdatePhoto(PhotoModel photo)
        {
            return new HttpResult<Response<PhotoModel>>(photoRuleEngine.UpdatePhoto(photo), Request);
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
            return new HttpResult<Response<PhotoModel>>(photoRuleEngine.DeletePhoto(photoID, modifiedOn), Request);
        }
    }
}