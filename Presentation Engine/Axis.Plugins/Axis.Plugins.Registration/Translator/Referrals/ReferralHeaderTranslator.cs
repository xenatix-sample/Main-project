using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Plugins.Registration.Models.Referrals;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator.Referrals
{
    public static class ReferralHeaderTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralHeaderViewModel ToViewModel(this ReferralHeaderModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralHeaderViewModel
            {
                ReferralHeaderID = entity.ReferralHeaderID,
                ContactID = entity.ContactID,
                ReferralStatusID = entity.ReferralStatusID,
                ReferralTypeID = entity.ReferralTypeID,
                ResourceTypeID = entity.ResourceTypeID,
                ReferralSourceID = entity.ReferralSourceID,
                ReferralOriginID = entity.ReferralOriginID,
                OrganizationID = entity.OrganizationID,
                ReferralOrganizationID = entity.ReferralOrganizationID,
                OtherOrganization = entity.OtherOrganization,
                ReferralCategorySourceID = entity.ReferralCategorySourceID,
                ReferralDate=entity.ReferralDate,
                ModifiedOn = entity.ModifiedOn,
                OtherSource = entity.OtherSource,
                IsLinkedToContact = entity.IsLinkedToContact,
                IsReferrerConvertedToCollateral = entity.IsReferrerConvertedToCollateral,
                ContactRelationShip = entity.ContactRelationShip,
                LivingWithClientStatus = entity.LivingWithClientStatus,
                DriverLicense = entity.DriverLicense,
                DriverLicenseStateID = entity.DriverLicenseStateID,
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralHeaderViewModel> ToViewModel(this Response<ReferralHeaderModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralHeaderViewModel>();
            var referralOutcomes = new List<ReferralHeaderViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralHeaderModel referralHeaderCome)
                {
                    var transformedModel = referralHeaderCome.ToViewModel();
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
        public static ReferralHeaderModel ToModel(this ReferralHeaderViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralHeaderModel
            {
                ReferralHeaderID = model.ReferralHeaderID,
                ContactID = model.ContactID,
                ReferralStatusID = model.ReferralStatusID,
                ReferralTypeID = model.ReferralTypeID,
                ResourceTypeID = model.ResourceTypeID,
                ReferralSourceID = model.ReferralSourceID,
                ReferralOriginID = model.ReferralOriginID,
                OrganizationID = model.OrganizationID,
                ReferralOrganizationID = model.ReferralOrganizationID,
                OtherOrganization = model.OtherOrganization,
                ReferralCategorySourceID = model.ReferralCategorySourceID,
                ReferralDate = model.ReferralDate,
                ModifiedOn = model.ModifiedOn,
                OtherSource = model.OtherSource,
                IsLinkedToContact = model.IsLinkedToContact,
                IsReferrerConvertedToCollateral = model.IsReferrerConvertedToCollateral,
                ContactRelationShip = model.ContactRelationShip,
                LivingWithClientStatus = model.LivingWithClientStatus,
                DriverLicense = model.DriverLicense,
                DriverLicenseStateID = model.DriverLicenseStateID
            };

            return entity;
        }
    }
}