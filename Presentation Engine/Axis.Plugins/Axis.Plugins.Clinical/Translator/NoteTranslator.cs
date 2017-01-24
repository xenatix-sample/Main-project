using Axis.Plugins.Clinical.Models;
using Axis.Model.Clinical;
using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.Plugins.Clinical.Translator
{
    /// <summary>
    /// 
    /// </summary>
    public static class NoteTranslator
    {
        /// <summary>
        /// To the ViewModel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static NoteViewModel ToViewModel(this NoteModel entity)
        {
            if (entity == null)
                return null;

            var model = new NoteViewModel
            {
                NoteID = entity.NoteID,
                Notes = entity.Notes,
                NoteTypeID = entity.NoteTypeID,
                ContactID = entity.ContactID,
                EncounterID = entity.EncounterID,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                NoteStatusID = entity.NoteStatusID,
                DocumentStatusID = entity.DocumentStatusID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<NoteViewModel> ToViewModel(this Response<NoteModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<NoteViewModel>();
            var notes = new List<NoteViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(NoteModel note)
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
        public static NoteModel ToModel(this NoteViewModel model)
        {
            if (model == null)
                return null;

            var entity = new NoteModel
            {
                NoteID = model.NoteID,
                Notes = model.Notes,
                NoteTypeID = model.NoteTypeID,
                ContactID = model.ContactID,
                EncounterID = model.EncounterID,
                TakenBy = model.TakenBy,
                TakenTime = model.TakenTime,
                NoteStatusID = model.NoteStatusID,
                DocumentStatusID = model.DocumentStatusID,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

    }
}
