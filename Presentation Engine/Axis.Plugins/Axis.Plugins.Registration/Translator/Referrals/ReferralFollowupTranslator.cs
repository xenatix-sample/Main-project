using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Plugins.Registration.Models.Referrals;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator.Referrals
{
    /// <summary>
    ///
    /// </summary>
    public static class ReferralFollowupTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralOutcomeDetailsViewModel ToViewModel(this ReferralOutcomeDetailsModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralOutcomeDetailsViewModel
            {
                ReferralOutcomeDetailID = entity.ReferralOutcomeDetailID,
                ReferralHeaderID = entity.ReferralHeaderID,
                FollowupExpected = entity.FollowupExpected,
                FollowupProviderID = entity.FollowupProviderID,
                FollowupDate = entity.FollowupDate,
                FollowupOutcome = entity.FollowupOutcome,
                IsAppointmentNotified = entity.IsAppointmentNotified,
                AppointmentNotificationMethod = entity.AppointmentNotificationMethod,
                Comments = entity.Comments,
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
        public static Response<ReferralOutcomeDetailsViewModel> ToViewModel(this Response<ReferralOutcomeDetailsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralOutcomeDetailsViewModel>();
            var referralOutcomes = new List<ReferralOutcomeDetailsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralOutcomeDetailsModel referralOutCome)
                {
                    var transformedModel = referralOutCome.ToViewModel();
                    referralOutcomes.Add(transformedModel);
                });

                model.DataItems = referralOutcomes;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralOutcomeDetailsModel ToModel(this ReferralOutcomeDetailsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralOutcomeDetailsModel
            {
                ReferralOutcomeDetailID = model.ReferralOutcomeDetailID,
                ReferralHeaderID = model.ReferralHeaderID,
                FollowupExpected = model.FollowupExpected,
                FollowupProviderID = model.FollowupProviderID,
                FollowupDate = model.FollowupDate,
                FollowupOutcome = model.FollowupOutcome,
                IsAppointmentNotified = model.IsAppointmentNotified,
                AppointmentNotificationMethod = model.AppointmentNotificationMethod,
                Comments = model.Comments,
                ForceRollback = model.ForceRollback,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}