using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Models.EligibilityDetermination;

namespace Axis.Plugins.ECI.Translator
{
    public static class EligibilityDeterminationTranslator
    {
        public static EligibilityDeterminationViewModel ToViewModel(this EligibilityDeterminationModel entity)
        {
            if (entity == null)
                return null;

            var model = new EligibilityDeterminationViewModel
            {
                EligibilityID = entity.EligibilityID,
                ContactID = entity.ContactID,
                EligibilityDate = entity.EligibilityDate,
                EligibilityTypeID = entity.EligibilityTypeID,
                EligibilityType = entity.EligibilityType,
                EligibilityDurationID = entity.EligibilityDurationID,
                EligibilityCategoryID = entity.EligibilityCategoryID,
                EligibilityCategory = entity.EligibilityCategory,
                AdjustedAge = entity.AdjustedAge,
                DOB = entity.DOB,
                Notes = entity.Notes,
                Members = entity.Members,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static EligibilityDeterminationModel ToModel(this EligibilityDeterminationViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new EligibilityDeterminationModel
            {
                EligibilityID = entity.EligibilityID,
                ContactID = entity.ContactID,
                EligibilityDate = entity.EligibilityDate,
                EligibilityTypeID = entity.EligibilityTypeID,
                EligibilityType = entity.EligibilityType,
                EligibilityDurationID = entity.EligibilityDurationID,
                EligibilityCategoryID = entity.EligibilityCategoryID,
                EligibilityCategory = entity.EligibilityCategory,
                AdjustedAge = entity.AdjustedAge,
                DOB = entity.DOB,
                Notes = entity.Notes,
                Members = entity.Members,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<EligibilityDeterminationViewModel> ToModel(this Response<EligibilityDeterminationModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<EligibilityDeterminationViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(EligibilityDeterminationModel eligibilityDeterminationModel)
                {
                    var transformedModel = eligibilityDeterminationModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<EligibilityDeterminationViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID = entity.ID
            };

            return model;
        }
    }
}
