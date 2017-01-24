using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    public static class ReferralConcernDetailTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralConcernDetailViewModel ToViewModel(this ReferralConcernDetailModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralConcernDetailViewModel
            {
                ReferralConcernDetailID = entity.ReferralConcernDetailID,
                ReferralAdditionalDetailID = entity.ReferralAdditionalDetailID,
                ReferralConcernID = entity.ReferralConcernID,
                ReferralPriorityID = entity.ReferralPriorityID,
                Diagnosis = entity.Diagnosis,
                ReferralConcern = entity.ReferralConcern,
                IsActive = entity.IsActive,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralConcernDetailViewModel> ToViewModel(this Response<ReferralConcernDetailModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralConcernDetailViewModel>();
            var referralConcernDetail = new List<ReferralConcernDetailViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralConcernDetailModel ReferralConcernDetailCome)
                {
                    var transformedModel = ReferralConcernDetailCome.ToViewModel();
                    referralConcernDetail.Add(transformedModel);
                });

                model.DataItems = referralConcernDetail;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralConcernDetailModel ToModel(this ReferralConcernDetailViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralConcernDetailModel
            {
                ReferralConcernDetailID = model.ReferralConcernDetailID,
                ReferralAdditionalDetailID = model.ReferralAdditionalDetailID,
                ReferralConcernID = model.ReferralConcernID,
                ReferralPriorityID = model.ReferralPriorityID,
                Diagnosis = model.Diagnosis,
                ReferralConcern = model.ReferralConcern,
                IsActive = model.IsActive,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}