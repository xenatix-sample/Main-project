using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Models;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.ECI.Translator
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProgressNoteTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ProgressNoteViewModel ToViewModel(this ProgressNoteModel entity){
            if (entity == null)
                return null;

            if (entity.StartTime == null)
            {
                entity.StartTime = new TimeSpan();
            }
            if (entity.EndTime == null)
            {
                entity.EndTime = new TimeSpan();
            }
            var model = new ProgressNoteViewModel
            {
                ProgressNoteID = entity.ProgressNoteID,
                NoteHeaderID = entity.NoteHeaderID,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                ContactMethodID = entity.ContactMethodID,
                ContactMethodOther = entity.ContactMethodOther,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                RelationshipTypeID = entity.RelationshipTypeID,
                Summary = entity.Summary,
                ReviewedSourceConcerns = entity.ReviewedSourceConcerns,
                ReviewedECIProcess = entity.ReviewedECIProcess,
                ReviewedECIServices = entity.ReviewedECIServices,
                ReviewedECIRequirements = entity.ReviewedECIRequirements,
                IsSurrogateParentNeeded = entity.IsSurrogateParentNeeded,
                Comments = entity.Comments,
                IsDischarged = entity.IsDischarged,
                StartTimeSecs = (entity.StartTime.Value.Days * 24 * 60 * 60 + 
                                entity.StartTime.Value.Hours *60 * 60 + 
                                entity.StartTime.Value.Minutes * 60 + 
                                entity.StartTime.Value.Seconds),
                EndTimeSecs = (entity.EndTime.Value.Days * 24 * 60 * 60 + 
                                entity.EndTime.Value.Hours *60 * 60 + 
                                entity.EndTime.Value.Minutes * 60 + 
                                entity.EndTime.Value.Seconds),
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ProgressNoteViewModel> ToViewModel(this Response<ProgressNoteModel> entity) {
            if (entity == null)
                return null;
            var model = entity.CloneResponse<ProgressNoteViewModel>();
            var noteList = new List<ProgressNoteViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ProgressNoteModel noteModel)
                {
                    var transformedModel = noteModel.ToViewModel();
                    noteList.Add(transformedModel);
                });
                model.DataItems = noteList;
            }
            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ProgressNoteModel ToModel(this ProgressNoteViewModel model){
            if (model == null)
                return null;

            var entity = new ProgressNoteModel
            {
                ProgressNoteID = model.ProgressNoteID,
                NoteHeaderID = model.NoteHeaderID,
                StartTime = TimeSpan.FromSeconds(model.StartTimeSecs),
                EndTime = TimeSpan.FromSeconds(model.EndTimeSecs),
                ContactMethodID = model.ContactMethodID,
                ContactMethodOther = model.ContactMethodOther,
                FirstName = model.FirstName,
                LastName = model.LastName,
                RelationshipTypeID = model.RelationshipTypeID,
                Summary = model.Summary,
                ReviewedSourceConcerns = model.ReviewedSourceConcerns,
                ReviewedECIProcess = model.ReviewedECIProcess,
                ReviewedECIServices = model.ReviewedECIServices,
                ReviewedECIRequirements = model.ReviewedECIRequirements,
                IsSurrogateParentNeeded = model.IsSurrogateParentNeeded,
                Comments = model.Comments,
                IsDischarged = model.IsDischarged,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }

    }
}
