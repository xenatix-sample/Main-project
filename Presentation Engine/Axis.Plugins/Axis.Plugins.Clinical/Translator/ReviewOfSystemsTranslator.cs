using Axis.Model.Clinical.ReviewOfSystems;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.ReviewOfSystems;
using System.Collections.Generic;

namespace Axis.Plugins.Clinical.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class ReviewOfSystemsTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReviewOfSystemsViewModel ToViewModel(this ReviewOfSystemsModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReviewOfSystemsViewModel
            {
                RoSID = entity.RoSID,
                ContactID = entity.ContactID,
                DateEntered = entity.DateEntered,
                ReviewdBy = entity.ReviewdBy,
                ReviewdByName = entity.ReviewdByName,
                AssessmentID = entity.AssessmentID,
                ResponseID = entity.ResponseID,
                IsReviewChanged = entity.IsReviewChanged,
                LastAssessmentOn = entity.LastAssessmentOn,
                ForceRollback = entity.ForceRollback,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReviewOfSystemsViewModel> ToViewModel(this Response<ReviewOfSystemsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReviewOfSystemsViewModel>();
            var rosList = new List<ReviewOfSystemsViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(ReviewOfSystemsModel ros)
            {
                var transformModel = ros.ToViewModel();
                rosList.Add(transformModel);
            });
            model.DataItems = rosList;

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReviewOfSystemsModel ToModel(this ReviewOfSystemsViewModel model)
        {
            if (model == null)
                return null;
            var entity = new ReviewOfSystemsModel
            {
                RoSID = model.RoSID,
                ContactID = model.ContactID,
                DateEntered = model.DateEntered,
                ReviewdBy = model.ReviewdBy,
                ReviewdByName = model.ReviewdByName,
                AssessmentID = model.AssessmentID,
                ResponseID = model.ResponseID,
                IsReviewChanged = model.IsReviewChanged,
                LastAssessmentOn = model.LastAssessmentOn,
                ForceRollback = model.ForceRollback,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }
    }
}