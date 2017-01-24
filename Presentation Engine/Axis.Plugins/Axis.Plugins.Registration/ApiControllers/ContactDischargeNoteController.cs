using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
  public  class ContactDischargeNoteController : BaseApiController
    {
         #region Private Variable

        private readonly IContactDischargeNoteRepository contactDischargeNoteRepository;

        #endregion

        public ContactDischargeNoteController(IContactDischargeNoteRepository contactDischargeNoteRepository)
        {
            this.contactDischargeNoteRepository = contactDischargeNoteRepository;
        }

        /// <summary>
        /// Get contact discharge note 
        /// </summary>
        /// <param name="contactDischargeNoteID">contact discharge note identifier</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactDischargeNoteViewModel> GetContactDischargeNote(long contactDischargeNoteID)
        {
            return contactDischargeNoteRepository.GetContactDischargeNote(contactDischargeNoteID);
        }

        /// <summary>
        /// Get contact discharge notes 
        /// </summary>
        /// <param name="contactDischargeNoteID">contact discharge note identifier</param>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactDischargeNoteViewModel> GetContactDischargeNotes(long contactDischargeNoteID,int noteTypeID)
        {
            return contactDischargeNoteRepository.GetContactDischargeNotes(contactDischargeNoteID, noteTypeID);
        }

        /// <summary>
        /// Adds the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ContactDischargeNoteViewModel> AddContactDischargeNote(ContactDischargeNoteViewModel contactDischargeNote)
        {
            return contactDischargeNoteRepository.AddContactDischargeNote(contactDischargeNote);
        }


        /// <summary>
        /// Updates the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ContactDischargeNoteViewModel> UpdateContactDischargeNote(ContactDischargeNoteViewModel contactDischargeNote)
        {
            return contactDischargeNoteRepository.UpdateContactDischargeNote(contactDischargeNote);
        }

        /// <summary>
        /// delete the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public void DeleteContactDischargeNote(long contactDischargeNoteID, DateTime modifiedOn)
        {
            contactDischargeNoteRepository.DeleteContactDischargeNote(contactDischargeNoteID, modifiedOn);
        }
    }
}
