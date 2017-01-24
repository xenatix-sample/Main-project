using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Admin.Translator
{
    public static class UserPhotoTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static UserPhotoViewModel ToViewModel(this UserPhotoModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserPhotoViewModel
            {
                UserPhotoID = entity.UserPhotoID,
                UserID = entity.UserID,
                PhotoID = entity.PhotoID,
                PhotoBLOB = entity.PhotoBLOB,
                ThumbnailBLOB = entity.ThumbnailBLOB,
                IsPrimary = entity.IsPrimary,
                ModifiedOn = entity.ModifiedOn,
                ForceRollback = entity.ForceRollback
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<UserPhotoViewModel> ToViewModel(this Response<UserPhotoModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<UserPhotoViewModel>();
            var userPhoto = new List<UserPhotoViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserPhotoModel photo)
                {
                    var transformedModel = photo.ToViewModel();
                    userPhoto.Add(transformedModel);
                });

                model.DataItems = userPhoto;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static UserPhotoModel ToModel(this UserPhotoViewModel model)
        {
            if (model == null)
                return null;

            var entity = new UserPhotoModel
            {
                UserPhotoID = model.UserPhotoID,
                UserID = model.UserID,
                PhotoID = model.PhotoID,
                PhotoBLOB = model.PhotoBLOB,
                ThumbnailBLOB = model.ThumbnailBLOB,
                IsPrimary = model.IsPrimary,
                ModifiedOn = model.ModifiedOn,
                ForceRollback = model.ForceRollback
            };

            return entity;
        }
    }
}