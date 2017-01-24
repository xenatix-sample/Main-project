using Axis.Model.Common;
using Axis.Model.Photo;
using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Helpers.Translator.Photo
{
    /// <summary>
    ///
    /// </summary>
    public static class PhotoTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static PhotoViewModel ToViewModel(this PhotoModel entity)
        {
            if (entity == null)
                return null;

            var model = new PhotoViewModel
            {
                PhotoID = entity.PhotoID,
                PhotoBLOB = entity.PhotoBLOB,
                ThumbnailBLOB = entity.ThumbnailBLOB,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<PhotoViewModel> ToViewModel(this Response<PhotoModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<PhotoViewModel>();
            var photo = new List<PhotoViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(PhotoModel photoModel)
                {
                    var transformedModel = photoModel.ToViewModel();
                    photo.Add(transformedModel);
                });

                model.DataItems = photo;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static PhotoModel ToModel(this PhotoViewModel model)
        {
            if (model == null)
                return null;

            var entity = new PhotoModel
            {
                PhotoID = model.PhotoID,
                PhotoBLOB = model.PhotoBLOB,
                ThumbnailBLOB = model.ThumbnailBLOB,
                TakenBy = model.TakenBy,
                TakenTime = model.TakenTime
            };

            return entity;
        }
    }
}