using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Clinical.Translator
{
    public static class NoteDetailsTranslator
    {
        /// <summary>
        /// To the ViewModel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static NoteDetailsViewModel ToViewModel(this NoteDetailsModel entity)
        {
            if (entity == null)
                return null;

            var model = new NoteDetailsViewModel
            {
                NoteID = entity.NoteID,
                Notes = entity.Notes,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<NoteDetailsViewModel> ToViewModel(this Response<NoteDetailsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<NoteDetailsViewModel>();
            var notes = new List<NoteDetailsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(NoteDetailsModel note)
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
        public static NoteDetailsModel ToModel(this NoteDetailsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new NoteDetailsModel
            {
                NoteID = model.NoteID,
                Notes = model.Notes,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}
