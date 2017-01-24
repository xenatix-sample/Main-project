using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class ContactPhotoTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ContactPhotoViewModel ToViewModel(this ContactPhotoModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactPhotoViewModel
            {
                ContactPhotoID = entity.ContactPhotoID,
                ContactID = entity.ContactID,
                PhotoID = entity.PhotoID,
                PhotoBLOB = entity.PhotoBLOB,
                ThumbnailBLOB = entity.ThumbnailBLOB,
                IsPrimary = entity.IsPrimary,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ContactPhotoViewModel> ToViewModel(this Response<ContactPhotoModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ContactPhotoViewModel>();
            var contactPhoto = new List<ContactPhotoViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactPhotoModel photo)
                {
                    var transformedModel = photo.ToViewModel();
                    contactPhoto.Add(transformedModel);
                });

                model.DataItems = contactPhoto;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ContactPhotoModel ToModel(this ContactPhotoViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactPhotoModel
            {
                ContactPhotoID = model.ContactPhotoID,
                ContactID = model.ContactID,
                PhotoID = model.PhotoID,
                PhotoBLOB = model.PhotoBLOB,
                ThumbnailBLOB = model.ThumbnailBLOB,
                IsPrimary = model.IsPrimary,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}