using Axis.Model.Clinical.Assessment;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.Assessment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Clinical.Translator
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssessmentTranslator
    {
        /// <summary>
        /// To the ViewModel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ClinicalAssessmentViewModel ToViewModel(this ClinicalAssessmentModel entity)
        {
            if (entity == null)
                return null;

            var model = new ClinicalAssessmentViewModel
            {
                ClinicalAssessmentID=entity.ClinicalAssessmentID,
                AssessmentID = entity.AssessmentID,
                ResponseID = entity.ResponseID,
                SectionID = entity.SectionID,
                ContactID = entity.ContactID,
                EncounterID = entity.EncounterID,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ClinicalAssessmentViewModel> ToViewModel(this Response<ClinicalAssessmentModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ClinicalAssessmentViewModel>();
            var notes = new List<ClinicalAssessmentViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ClinicalAssessmentModel note)
                {
                    var transformedModel = note.ToViewModel();
                    notes.Add(transformedModel);
                });

                model.DataItems = notes;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ClinicalAssessmentModel ToModel(this ClinicalAssessmentViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ClinicalAssessmentModel
            {
                
                ClinicalAssessmentID=model.ClinicalAssessmentID,
                AssessmentID = model.AssessmentID,
                ResponseID=model.ResponseID,
                SectionID = model.SectionID,
                ContactID = model.ContactID,
                EncounterID = model.EncounterID,
                TakenBy = model.TakenBy,
                TakenTime = model.TakenTime,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

    }
}
