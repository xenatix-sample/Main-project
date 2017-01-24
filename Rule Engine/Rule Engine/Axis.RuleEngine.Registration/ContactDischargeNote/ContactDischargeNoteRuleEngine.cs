using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration
{
    public class ContactDischargeNoteRuleEngine : IContactDischargeNoteRuleEngine
    {
         
        /// <summary>
        /// The contact discharge note service
        /// </summary>
        private readonly IContactDischargeNoteService contactDischargeNoteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDischargeNoteRuleEngine" /> class.
        /// </summary>
        /// <param name="ContactDischargeNoteRuleEngine">The contact discharge note service.</param>
        public ContactDischargeNoteRuleEngine(IContactDischargeNoteService contactDischargeNoteService)
        {
            this.contactDischargeNoteService = contactDischargeNoteService;
        }

        /// <summary>
        /// Gets the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> GetContactDischargeNote(long contactDischargeNoteID)
        {
            return contactDischargeNoteService.GetContactDischargeNote(contactDischargeNoteID);
        }

        /// <summary>
        /// Gets the contact discharge notes.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> GetContactDischargeNotes(long contactDischargeNoteID,int noteTypeID)
        {
            return contactDischargeNoteService.GetContactDischargeNotes(contactDischargeNoteID, noteTypeID);
        }

        /// <summary>
        /// Adds the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> AddContactDischargeNote(ContactDischargeNote contactDischargeNote)
        {
            return contactDischargeNoteService.AddContactDischargeNote(contactDischargeNote);
        }

        /// <summary>
        /// Updates the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> UpdateContactDischargeNote(ContactDischargeNote contactDischargeNote)
        {
            return contactDischargeNoteService.UpdateContactDischargeNote(contactDischargeNote);
        }

        /// <summary>
        /// delete the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> DeleteContactDischargeNote(long contactDischargeNoteID, DateTime modifiedOn)
        {
            return contactDischargeNoteService.DeleteContactDischargeNote(contactDischargeNoteID, modifiedOn);
        }
    }
}
