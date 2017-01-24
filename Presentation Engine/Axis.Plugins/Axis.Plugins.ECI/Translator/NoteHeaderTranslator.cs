using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Models;
using System.Collections.Generic;

namespace Axis.Plugins.ECI.Translator
{
    public static class NoteHeaderTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static NoteHeaderViewModel ToViewModel(this NoteHeaderModel entity)
        {
            if (entity == null)
                return null;
            var model = new NoteHeaderViewModel
            {
                NoteHeaderID = entity.NoteHeaderID,
                ContactID = entity.ContactID,
                NoteTypeID = entity.NoteTypeID,
                TakenTime = entity.TakenTime,
                TakenBy = entity.TakenBy,
                Discharge = entity.Discharge.ToViewModel(),
                ProgressNote = entity.ProgressNote.ToViewModel(),
                ProgressNoteAssessment = entity.ProgressNoteAssessment.ToViewModel(),
                ModifiedOn = entity.ModifiedOn
            };
            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<NoteHeaderViewModel> ToViewModel(this Response<NoteHeaderModel> entity)
        {
            if (entity == null)
                return null;
            var model = entity.CloneResponse<NoteHeaderViewModel>();
            var noteHeaderList = new List<NoteHeaderViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(NoteHeaderModel noteHeaderModel)
                {
                    var transformedModel = noteHeaderModel.ToViewModel();
                    noteHeaderList.Add(transformedModel);
                });
                model.DataItems = noteHeaderList;
            }
            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static NoteHeaderModel ToModel(this NoteHeaderViewModel model)
        {
            if (model == null)
                return null;

            var entity = new NoteHeaderModel
            {
                NoteHeaderID = model.NoteHeaderID,
                ContactID = model.ContactID,
                NoteTypeID = model.NoteTypeID,
                TakenTime = model.TakenTime,
                TakenBy = model.TakenBy,
                Discharge = model.Discharge.ToModel(),
                ProgressNote = model.ProgressNote.ToModel(),
                ProgressNoteAssessment = model.ProgressNoteAssessment.ToModel(),
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }
    }
}
