using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{

    /// <summary>
    /// Translates Contact discharge note to view model
    /// </summary>
    public static class ContactDischargeNoteTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ContactDischargeNoteViewModel ToViewModel(this  ContactDischargeNote entity)
        {
            if (entity == null)
                return null;

            var model = new ContactDischargeNoteViewModel
            {
                ContactID = entity.ContactID,
                ContactDischargeNoteID = entity.ContactDischargeNoteID,
                NoteTypeID = entity.NoteTypeID,
                ContactAdmissionID = entity.ContactAdmissionID,
                DischargeReasonID = entity.DischargeReasonID,
                DischargeDate = entity.DischargeDate,
                NoteText = entity.NoteText,
                SignatureStatusID = entity.SignatureStatusID,
                IsDeceased = entity.IsDeceased,
                DeceasedDate = entity.DeceasedDate
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ContactDischargeNoteViewModel> ToViewModel(this Response<ContactDischargeNote> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ContactDischargeNoteViewModel>();
            var contactDischargeNoteList = new List<ContactDischargeNoteViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactDischargeNote contactDischargeNote)
                {
                    var transformedModel = contactDischargeNote.ToViewModel();
                    contactDischargeNoteList.Add(transformedModel);
                });

                model.DataItems = contactDischargeNoteList;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ContactDischargeNote ToModel(this ContactDischargeNoteViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactDischargeNote
            {
                ContactID = model.ContactID,
                ContactDischargeNoteID = model.ContactDischargeNoteID,
                NoteTypeID = model.NoteTypeID,
                ContactAdmissionID = model.ContactAdmissionID,
                DischargeReasonID = model.DischargeReasonID,
                DischargeDate = model.DischargeDate,
                IsDeceased = model.IsDeceased,
                DeceasedDate = model.DeceasedDate,
                NoteText = model.NoteText,
                SignatureStatusID = model.SignatureStatusID
            };

            return entity;
        }
    }
}
