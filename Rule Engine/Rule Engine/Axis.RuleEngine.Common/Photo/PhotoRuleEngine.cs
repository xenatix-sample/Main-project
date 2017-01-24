using System;
using Axis.Model.Common;
using Axis.Model.Photo;
using Axis.Service.Common.Photo;

namespace Axis.RuleEngine.Common.Photo
{
    /// <summary>
    ///
    /// </summary>
    public class PhotoRuleEngine : IPhotoRuleEngine
    {
        /// <summary>
        /// The photo service
        /// </summary>
        private IPhotoService photoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoRuleEngine"/> class.
        /// </summary>
        /// <param name="photoService">The photo service.</param>
        public PhotoRuleEngine(IPhotoService photoService)
        {
            this.photoService = photoService;
        }

        /// <summary>
        /// Gets the photo.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<PhotoModel> GetPhoto(long assessmentId)
        {
            return photoService.GetPhoto(assessmentId);
        }

        /// <summary>
        /// Adds the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        public Response<PhotoModel> AddPhoto(PhotoModel photo)
        {
            return photoService.AddPhoto(photo);
        }

        /// <summary>
        /// Updates the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        public Response<PhotoModel> UpdatePhoto(PhotoModel photo)
        {
            return photoService.UpdatePhoto(photo);
        }

        /// <summary>
        /// Deletes the photo.
        /// </summary>
        /// <param name="photoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<PhotoModel> DeletePhoto(long photoID, DateTime modifiedOn)
        {
            return photoService.DeletePhoto(photoID, modifiedOn);
        }
    }
}