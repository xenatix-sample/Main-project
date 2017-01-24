using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Plugins.Registration.Models.Referrals;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator.Referrals
{
    public static class ReferralDemographicsTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralDemographicsViewModel ToViewModel(this ReferralDemographicsModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralDemographicsViewModel
            {
                ReferralID = entity.ReferralID,
                ContactTypeID = entity.ContactTypeID,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                SuffixID = entity.SuffixID,
                MPI = entity.MPI,
                TitleID = entity.TitleID,
                Middle = entity.Middle,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralDemographicsViewModel> ToViewModel(this Response<ReferralDemographicsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralDemographicsViewModel>();
            var referralDemographics = new List<ReferralDemographicsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralDemographicsModel referralDemographicsCome)
                {
                    var transformedModel = referralDemographicsCome.ToViewModel();
                    referralDemographics.Add(transformedModel);
                });

                model.DataItems = referralDemographics;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralDemographicsModel ToModel(this ReferralDemographicsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralDemographicsModel
            {
                ReferralID = model.ReferralID,
                ContactTypeID = model.ContactTypeID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                SuffixID = model.SuffixID,
                MPI = model.MPI,
                TitleID = model.TitleID,
                Middle = model.Middle,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}