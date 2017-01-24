using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// Converts Model to ViewModel and vice versa
    /// </summary>
    public static class ReferralContactTranslator
    {
        /// <summary>
        /// Convert ReferralContactModel Model to ReferralContactViewModel ViewModel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ReferralContactViewModel ToViewModel(this ReferralContactModel entity)
        {
            if (entity == null)
                return null;
            var model = new ReferralViewModel
            {
                ReferralID = entity.ReferralID,
                ReferralContactID = entity.ReferralContactID,
                ContactID = entity.ContactID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// Convert ReferralContactModel Model to ReferralContactViewModel ViewModel for response
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Response<ReferralContactViewModel> ToViewModel(this Response<ReferralContactModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralContactViewModel>();
            var referralList = new List<ReferralContactViewModel>();
            if (entity.DataItems == null)
                return null;

            entity.DataItems.ForEach(delegate(ReferralContactModel referralContactModel)
            {
                var transformedModel = referralContactModel.ToViewModel();
                referralList.Add(transformedModel);
            });
            model.DataItems = referralList;

            return model;
        }

        /// <summary>
        /// Convert ReferralContactViewModel ViewModel to ReferralContactModel Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ReferralContactModel ToModel(this ReferralContactViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralModel
            {
                ReferralID = model.ReferralID,
                ReferralContactID = model.ReferralContactID,
                ContactID = model.ContactID,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}