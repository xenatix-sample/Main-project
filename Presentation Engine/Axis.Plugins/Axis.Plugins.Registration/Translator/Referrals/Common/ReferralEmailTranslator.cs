using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using Axis.Plugins.Registration.Models.Referrals.Common;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator.Referrals.Common
{
    public static class ReferralEmailTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralEmailViewModel ToViewModel(this ReferralEmailModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralEmailViewModel
            {
                ReferralEmailID = entity.ReferralEmailID,
                ReferralID = entity.ReferralID,
                EmailPermissionID = entity.EmailPermissionID,
                Email = entity.Email,
                EmailID = entity.EmailID,
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
        public static Response<ReferralEmailViewModel> ToViewModel(this Response<ReferralEmailModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralEmailViewModel>();
            var referralEmail = new List<ReferralEmailViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralEmailModel referralEmailCome)
                {
                    var transformedModel = referralEmailCome.ToViewModel();
                    referralEmail.Add(transformedModel);
                });

                model.DataItems = referralEmail;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralEmailModel ToModel(this ReferralEmailViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralEmailModel
            {
                ReferralEmailID = model.ReferralEmailID,
                ReferralID = model.ReferralID,
                EmailPermissionID = model.EmailPermissionID,
                Email = model.Email,
                EmailID = model.EmailID,
                IsActive= model.IsActive,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static List<ReferralEmailModel> ToModel(this List<ReferralEmailViewModel> model)
        {
            if (model == null)
                return null;

            var entity = new List<ReferralEmailModel>();

            model.ForEach(delegate(ReferralEmailViewModel email)
            {
                var transformedModel = email.ToModel();
                entity.Add(transformedModel);
            });

            return entity;
        }
    }
}