using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using Axis.Plugins.Registration.Models.Referrals.Common;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator.Referrals.Common
{
    public static class ReferralPhoneTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralPhoneViewModel ToViewModel(this ReferralPhoneModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralPhoneViewModel
            {
                ReferralPhoneID = entity.ReferralPhoneID,
                ReferralID = entity.ReferralID,
                PhonePermissionID = entity.PhonePermissionID,
                PhoneID = entity.PhoneID,
                PhoneTypeID = entity.PhoneTypeID,
                Number = entity.Number,
                Extension = entity.Extension,
                IsActive= entity.IsActive,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralPhoneViewModel> ToViewModel(this Response<ReferralPhoneModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralPhoneViewModel>();
            var referralPhone = new List<ReferralPhoneViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralPhoneModel referralPhoneCome)
                {
                    var transformedModel = referralPhoneCome.ToViewModel();
                    referralPhone.Add(transformedModel);
                });

                model.DataItems = referralPhone;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralPhoneModel ToModel(this ReferralPhoneViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralPhoneModel
            {
                ReferralPhoneID = model.ReferralPhoneID,
                ReferralID = model.ReferralID,
                PhonePermissionID = model.PhonePermissionID,
                PhoneID = model.PhoneID,
                PhoneTypeID = model.PhoneTypeID,
                Number = model.Number,
                Extension = model.Extension,
                IsActive=model.IsActive,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static List<ReferralPhoneModel> ToModel(this List<ReferralPhoneViewModel> model)
        {
            if (model == null)
                return null;

            var entity = new List<ReferralPhoneModel>();

            model.ForEach(delegate(ReferralPhoneViewModel phone)
            {
                var transformedModel = phone.ToModel();
                entity.Add(transformedModel);
            });

            return entity;
        }
    }
}