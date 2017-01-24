using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using System;

namespace Axis.Plugins.Registration.Repository
{
   public interface IContactDischargeNoteRepository
    {
        /// <summary>
        /// Gets the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <returns></returns>
        Response<ContactDischargeNoteViewModel> GetContactDischargeNote(long contactDischargeNoteID);

        /// <summary>
        /// Gets the contact discharge notes.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <returns></returns>
        Response<ContactDischargeNoteViewModel> GetContactDischargeNotes(long contactDischargeNoteID,int noteTypeID);

        /// <summary>
        /// Adds the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        Response<ContactDischargeNoteViewModel> AddContactDischargeNote(ContactDischargeNoteViewModel contactDischargeNote);

        /// <summary>
        /// Updates the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        Response<ContactDischargeNoteViewModel> UpdateContactDischargeNote(ContactDischargeNoteViewModel contactDischargeNote);

        /// <summary>
        /// delete the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        Response<ContactDischargeNoteViewModel> DeleteContactDischargeNote(long contactDischargeNoteID, DateTime modifiedOn);
    }
}
