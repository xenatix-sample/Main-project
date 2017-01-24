using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Plugins.Registration.Models.Referrals;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator.Referrals
{
    /// <summary>
    ///
    /// </summary>
    public static class ReferralForwardedTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralForwardedViewModel ToViewModel(this ReferralForwardedModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralForwardedViewModel
            {
                ReferralForwardedDetailID = entity.ReferralForwardedDetailID,
                ReferralHeaderID = entity.ReferralHeaderID,
                SendingReferralToID = entity.SendingReferralToID,
                UserID = entity.UserID,
                ReferralSentDate = entity.ReferralSentDate,
                Comments = entity.Comments,
                OrganizationID = entity.OrganizationID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralForwardedViewModel> ToViewModel(this Response<ReferralForwardedModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralForwardedViewModel>();
            var referralForwardedList = new List<ReferralForwardedViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralForwardedModel referralForwarded)
                {
                    var transformedModel = referralForwarded.ToViewModel();
                    referralForwardedList.Add(transformedModel);
                });

                model.DataItems = referralForwardedList;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralForwardedModel ToModel(this ReferralForwardedViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralForwardedModel
            {
                ReferralForwardedDetailID = model.ReferralForwardedDetailID,
                ReferralHeaderID = model.ReferralHeaderID,
                SendingReferralToID = model.SendingReferralToID,
                UserID = model.UserID,
                ReferralSentDate = model.ReferralSentDate,
                Comments = model.Comments,
                OrganizationID = model.OrganizationID,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}