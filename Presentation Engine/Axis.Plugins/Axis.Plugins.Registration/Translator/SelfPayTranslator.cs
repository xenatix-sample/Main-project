using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// Translates self pay model to view model
    /// </summary>
    public static class SelfPayTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static SelfPayViewModel ToViewModel(this  SelfPayModel entity)
        {
            if (entity == null)
                return null;

            var model = new SelfPayViewModel
            {
                ContactID = entity.ContactID,
                SelfPayID = entity.SelfPayID,
                EffectiveDate=entity.EffectiveDate,
                ExpirationDate=entity.ExpirationDate,
                OrganizationDetailID=entity.OrganizationDetailID,
                SelfPayAmount = entity.SelfPayAmount,
                IsPercent = entity.IsPercent,
                ISChildInConservatorship=entity.ISChildInConservatorship,
                IsNotAttested = entity.IsNotAttested,
                IsEnrolledInPublicBenefits=entity.IsEnrolledInPublicBenefits,
                IsRequestingReconsideration=entity.IsRequestingReconsideration,
                IsNotGivingConsent = entity.IsNotGivingConsent,
                IsOtherChildEnrolled = entity.IsOtherChildEnrolled,
                IsApplyingForPublicBenefits = entity.IsApplyingForPublicBenefits,
                IsReconsiderationOfAdjustment = entity.IsReconsiderationOfAdjustment,
                IsViewSelfPay=entity.IsViewSelfPay
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<SelfPayViewModel> ToViewModel(this Response<SelfPayModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<SelfPayViewModel>();
            var selfPayList = new List<SelfPayViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(SelfPayModel contactDischargeNote)
                {
                    var transformedModel = contactDischargeNote.ToViewModel();
                    selfPayList.Add(transformedModel);
                });

                model.DataItems = selfPayList;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static SelfPayModel ToModel(this SelfPayViewModel model)
        {
            if (model == null)
                return null;

            var entity = new SelfPayModel
            {
                ContactID = model.ContactID,
                SelfPayID = model.SelfPayID,
                EffectiveDate = model.EffectiveDate,
                ExpirationDate = model.ExpirationDate,
                OrganizationDetailID = model.OrganizationDetailID,
                SelfPayAmount = model.SelfPayAmount,
                IsPercent = model.IsPercent,
                ISChildInConservatorship = model.ISChildInConservatorship,
                IsNotAttested = model.IsNotAttested,
                IsEnrolledInPublicBenefits = model.IsEnrolledInPublicBenefits,
                IsRequestingReconsideration = model.IsRequestingReconsideration,
                IsNotGivingConsent = model.IsNotGivingConsent,
                IsOtherChildEnrolled = model.IsOtherChildEnrolled,
                IsApplyingForPublicBenefits = model.IsApplyingForPublicBenefits,
                IsReconsiderationOfAdjustment = model.IsReconsiderationOfAdjustment,
                IsViewSelfPay = model.IsViewSelfPay
            };

            return entity;
        }
    }
}
