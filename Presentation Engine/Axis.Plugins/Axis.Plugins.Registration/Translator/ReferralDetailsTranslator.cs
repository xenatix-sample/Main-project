using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Plugins.Registration.Models.Referrals;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReferralDetailsTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralDetailsViewModel ToViewModel(this ReferralDetailsModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralDetailsViewModel
            {
                ReferralHeaderID = entity.ReferralHeaderID,
                FirstName = entity.FirstName,
                ContactID = entity.ContactID,
                LastName = entity.LastName,
                PhoneNumber = entity.PhoneNumber,
                ReferralDate = entity.ReferralDate,
                ReferralSourceID = entity.ReferralSourceID,
                ReferralConcern = entity.ReferralConcern,
                ModifiedOn = entity.ModifiedOn,
                IsReferrerConvertedToCollateral = entity.IsReferrerConvertedToCollateral
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralDetailsViewModel> ToViewModel(this Response<ReferralDetailsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralDetailsViewModel>();
            var referral = new List<ReferralDetailsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (ReferralDetailsModel referralCome)
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
        public static ReferralDetailsModel ToModel(this ReferralDetailsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralDetailsModel
            {
                ReferralHeaderID = model.ReferralHeaderID,
                FirstName = model.FirstName,
                ContactID = model.ContactID,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                ReferralDate = model.ReferralDate,
                ReferralSourceID = model.ReferralSourceID,
                ReferralConcern = model.ReferralConcern,
                ModifiedOn = model.ModifiedOn,
                IsReferrerConvertedToCollateral = model.IsReferrerConvertedToCollateral
            };

            return entity;
        }
    }
}