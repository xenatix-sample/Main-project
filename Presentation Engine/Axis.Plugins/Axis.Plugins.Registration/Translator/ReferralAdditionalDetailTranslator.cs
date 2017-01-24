using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    public static class ReferralAdditionalDetailTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralAdditionalDetailViewModel ToViewModel(this ReferralAdditionalDetailModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralAdditionalDetailViewModel
            {
                ReferralAdditionalDetailID = entity.ReferralAdditionalDetailID,
                ReferralHeaderID = entity.ReferralHeaderID,
                ContactID = entity.ContactID,
                ReasonforCare = entity.ReasonforCare,
                IsTransferred = entity.IsTransferred,
                IsHousingProgram = entity.IsHousingProgram,
                HousingDescription = entity.HousingDescription,
                IsEligibleforFurlough = entity.IsEligibleforFurlough,
                IsReferralDischargeOrTransfer = entity.IsReferralDischargeOrTransfer,
                IsConsentRequired = entity.IsConsentRequired,
                Comments = entity.Comments,
                AdditionalConcerns = entity.AdditionalConcerns,
                HeaderContactID = entity.HeaderContactID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralAdditionalDetailViewModel> ToViewModel(this Response<ReferralAdditionalDetailModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralAdditionalDetailViewModel>();
            var referral = new List<ReferralAdditionalDetailViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralAdditionalDetailModel referralCome)
                {
                    var transformedModel = referralCome.ToViewModel();
                    referral.Add(transformedModel);
                });

                model.DataItems = referral;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralAdditionalDetailModel ToModel(this ReferralAdditionalDetailViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralAdditionalDetailModel
            {
                ReferralAdditionalDetailID = model.ReferralAdditionalDetailID,
                ReferralHeaderID = model.ReferralHeaderID,
                ContactID = model.ContactID,
                ReasonforCare = model.ReasonforCare,
                IsTransferred = model.IsTransferred,
                IsHousingProgram = model.IsHousingProgram,
                HousingDescription = model.HousingDescription,
                IsEligibleforFurlough = model.IsEligibleforFurlough,
                IsReferralDischargeOrTransfer = model.IsReferralDischargeOrTransfer,
                IsConsentRequired = model.IsConsentRequired,
                Comments = model.Comments,
                AdditionalConcerns = model.AdditionalConcerns,
                HeaderContactID = model.HeaderContactID,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}