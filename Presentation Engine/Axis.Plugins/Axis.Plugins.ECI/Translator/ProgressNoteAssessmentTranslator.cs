using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.ECI.Translator
{
    public static class ProgressNoteAssessmentTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ProgressNoteAssessmentViewModel ToViewModel(this ProgressNoteAssessmentModel entity)
        {
            if (entity == null)
                return null;
            if (entity.NoteAssessmentTime == null)
            {
                entity.NoteAssessmentTime = new TimeSpan();
            }
            var model = new ProgressNoteAssessmentViewModel
            {
                ScheduleNoteAssessmentID = entity.ScheduleNoteAssessmentID,
                NoteAssessmentDate = entity.NoteAssessmentDate,
                NoteAssessmentTime = entity.NoteAssessmentTime,
                LocationID = entity.LocationID,
                Location=entity.Location,
                ProviderID = entity.ProviderID,
                MembersInvited = entity.MembersInvited,
                ProgressNoteID = entity.ProgressNoteID,
                NoteAssessmentTimeSecs = (entity.NoteAssessmentTime.Value.Days * 24 * 60 * 60 +
                                entity.NoteAssessmentTime.Value.Hours * 60 * 60 +
                                entity.NoteAssessmentTime.Value.Minutes * 60 +
                                entity.NoteAssessmentTime.Value.Seconds),
                ModifiedOn = entity.ModifiedOn
            };
            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ProgressNoteAssessmentViewModel> ToViewModel(this Response<ProgressNoteAssessmentModel> entity)
        {
            if (entity == null)
                return null;
            var model = entity.CloneResponse<ProgressNoteAssessmentViewModel>();
            var noteList = new List<ProgressNoteAssessmentViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ProgressNoteAssessmentModel noteModel)
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
        public static ProgressNoteAssessmentModel ToModel(this ProgressNoteAssessmentViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ProgressNoteAssessmentModel
            {
                ScheduleNoteAssessmentID = model.ScheduleNoteAssessmentID,
                NoteAssessmentDate = model.NoteAssessmentDate,
                NoteAssessmentTime = TimeSpan.FromSeconds(model.NoteAssessmentTimeSecs),
                LocationID = model.LocationID,
                Location=model.Location,
                ProviderID = model.ProviderID,
                MembersInvited = model.MembersInvited,
                ProgressNoteID = model.ProgressNoteID,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }
    }
}
