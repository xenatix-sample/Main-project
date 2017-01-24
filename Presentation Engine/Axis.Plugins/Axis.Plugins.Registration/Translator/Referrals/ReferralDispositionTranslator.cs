using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Plugins.Registration.Models.Referrals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Translator.Referrals
{
    public static class ReferralDispositionTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralDispositionViewModel ToViewModel(this ReferralDispositionModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralDispositionViewModel
            {
                ReferralDispositionDetailID = entity.ReferralDispositionDetailID,
                ReferralHeaderID = entity.ReferralHeaderID,
                ReferralDispositionID = entity.ReferralDispositionID,
                ReasonforDenial = entity.ReasonforDenial,
                ReferralDispositionOutcomeID = entity.ReferralDispositionOutcomeID,
                AdditionalNotes = entity.AdditionalNotes,
                UserID = entity.UserID,
                DispositionDate = entity.DispositionDate,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralDispositionViewModel> ToViewModel(this Response<ReferralDispositionModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralDispositionViewModel>();
            var referralOutcomes = new List<ReferralDispositionViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralDispositionModel referralDispositionCome)
                {
                    var transformedModel = referralDispositionCome.ToViewModel();
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
        public static ReferralDispositionModel ToModel(this ReferralDispositionViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralDispositionModel
            {
                ReferralDispositionDetailID = model.ReferralDispositionDetailID,
                ReferralHeaderID = model.ReferralHeaderID,
                ReferralDispositionID = model.ReferralDispositionID,
                ReasonforDenial = model.ReasonforDenial,
                ReferralDispositionOutcomeID = model.ReferralDispositionOutcomeID,
                AdditionalNotes = model.AdditionalNotes,
                UserID = model.UserID,
                DispositionDate = model.DispositionDate,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}
