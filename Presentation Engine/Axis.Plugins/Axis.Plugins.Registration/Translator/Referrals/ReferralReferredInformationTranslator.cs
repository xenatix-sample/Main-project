using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Plugins.Registration.Models.Referrals;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator.Referrals
{
    /// <summary>
    /// Referral referred to information translator
    /// </summary>
    public static class ReferralReferredInformationTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralReferredInformationViewModel ToViewModel(this ReferralReferredInformationModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralReferredInformationViewModel
            {
                ReferredToDetailID = entity.ReferredToDetailID,              
                ActionTaken = entity.ActionTaken,
                Comments = entity.Comments,
                UserID = entity.UserID,
                ReferredDateTime = entity.ReferredDateTime,
                OrganizationID = entity.OrganizationID,
                ContactNo = entity.ContactNo,
                ReferralHeaderID = entity.ReferralHeaderID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralReferredInformationViewModel> ToViewModel(this Response<ReferralReferredInformationModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralReferredInformationViewModel>();
            var referredInformation = new List<ReferralReferredInformationViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralReferredInformationModel information)
                {
                    var transformedModel = information.ToViewModel();
                    referredInformation.Add(transformedModel);
                });

                model.DataItems = referredInformation;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralReferredInformationModel ToModel(this ReferralReferredInformationViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralReferredInformationModel
            {
                ReferredToDetailID = model.ReferredToDetailID,                
                ActionTaken = model.ActionTaken,
                Comments = model.Comments,
                UserID = model.UserID,
                ReferredDateTime = model.ReferredDateTime,
                OrganizationID = model.OrganizationID,
                ContactNo = model.ContactNo,
                ReferralHeaderID = model.ReferralHeaderID,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}