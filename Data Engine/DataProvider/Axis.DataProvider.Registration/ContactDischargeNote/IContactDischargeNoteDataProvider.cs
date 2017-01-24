using Axis.Model.Common;
using Axis.Model.Registration;
using System;

namespace Axis.DataProvider.Registration
{
    public interface IContactDischargeNoteDataProvider
    {
        /// <summary>
        /// Gets the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <returns></returns>
        Response<ContactDischargeNote> GetContactDischargeNote(long contactDischargeNoteID);

        /// <summary>
        /// Gets the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <returns></returns>
        Response<ContactDischargeNote> GetContactDischargeNotes(long contactDischargeNoteID,int noteTypeID);

        /// <summary>
        /// Adds the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        Response<ContactDischargeNote> AddContactDischargeNote(ContactDischargeNote contactDischargeNote);


        /// <summary>
        /// Updates the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        Response<ContactDischargeNote> UpdateContactDischargeNote(ContactDischargeNote contactDischargeNote);

        /// <summary>
        /// delete the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        Response<ContactDischargeNote> DeleteContactDischargeNote(long contactDischargeNoteID, DateTime modifiedOn);
    }
}
