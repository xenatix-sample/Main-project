using Axis.Model.Clinical.PresentIllness;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.PresentIllness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Clinical.Translator
{
    public static class PresentIllnessTranslator
    {
        public static PresentIllnessViewModel ToViewModel(this PresentIllnessModel entity)
        {
            if (entity == null)
                return null;

            var model = new PresentIllnessViewModel
            {
                EncounterID = entity.EncounterID,
                ContactID = entity.ContactID,
                HPIID = entity.HPIID,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static PresentIllnessDetailViewModel ToViewModel(this PresentIllnessDetailModel entity)
        {
            if (entity == null)
                return null;

            var model = new PresentIllnessDetailViewModel
            {
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                ContactID = entity.ContactID,
                HPIID = entity.HPIID,
                HPIDetailID = entity.HPIDetailID,
                Comment = entity.Comment,
                Location = entity.Location,
                Quality = entity.Quality,
                HPISeverityID = entity.HPISeverityID,
                Duration = entity.Duration,
                Timing = entity.Timing,
                Context = entity.Context,
                Modifyingfactors = entity.Modifyingfactors,
                Symptoms = entity.Symptoms,
                Conditions = entity.Conditions,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static PresentIllnessModel ToModel(this PresentIllnessViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new PresentIllnessModel
            {
                EncounterID = entity.EncounterID,
                ContactID = entity.ContactID,
                HPIID = entity.HPIID,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static PresentIllnessDetailModel ToModel(this PresentIllnessDetailViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new PresentIllnessDetailModel
            {
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                ContactID = entity.ContactID,
                HPIID = entity.HPIID,
                HPIDetailID = entity.HPIDetailID,
                Comment = entity.Comment,
                Location = entity.Location,
                Quality = entity.Quality,
                HPISeverityID = entity.HPISeverityID,
                Duration = entity.Duration,
                Timing = entity.Timing,
                Context = entity.Context,
                Modifyingfactors = entity.Modifyingfactors,
                Symptoms = entity.Symptoms,
                Conditions = entity.Conditions,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<PresentIllnessViewModel> ToModel(this Response<PresentIllnessModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<PresentIllnessViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(PresentIllnessModel allergyModel)
                {
                    var transformedModel = allergyModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<PresentIllnessViewModel>
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

        public static Response<PresentIllnessDetailViewModel> ToModel(this Response<PresentIllnessDetailModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<PresentIllnessDetailViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(PresentIllnessDetailModel hpiModel)
                {
                    var transformedModel = hpiModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<PresentIllnessDetailViewModel>
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
