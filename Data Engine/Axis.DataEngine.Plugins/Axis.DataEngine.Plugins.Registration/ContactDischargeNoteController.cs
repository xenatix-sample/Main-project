using System.Web.Http;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Helpers.Controllers;
using System;

namespace Axis.DataEngine.Plugins.Registration
{
    public class ContactDischargeNoteController : BaseApiController
    {
          /// <summary>
        /// The _contactDischargeNoteDataProvider data provider
        /// </summary>
        readonly IContactDischargeNoteDataProvider _contactDischargeNoteDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDischargeNoteController"/> class.
        /// </summary>
        /// <param name="contactDischargeNoteDataProvider">The contact discharge note data provider.</param>
        public ContactDischargeNoteController(IContactDischargeNoteDataProvider contactDischargeNoteDataProvider)
        {
            _contactDischargeNoteDataProvider = contactDischargeNoteDataProvider;
        }

        /// <summary>
        /// Gets the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <returns></returns>
        public IHttpActionResult GetContactDischargeNote(long contactDischargeNoteID)
        {
            return new HttpResult<Response<ContactDischargeNote>>(_contactDischargeNoteDataProvider.GetContactDischargeNote(contactDischargeNoteID), Request);
        }

        /// <summary>
        /// Gets the contact discharge notes.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <returns></returns>
        public IHttpActionResult GetContactDischargeNotes(long contactDischargeNoteID,int noteTypeID)
        {
            return new HttpResult<Response<ContactDischargeNote>>(_contactDischargeNoteDataProvider.GetContactDischargeNotes(contactDischargeNoteID, noteTypeID), Request);
        }

        /// <summary>
        /// Adds the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        public IHttpActionResult AddContactDischargeNote(ContactDischargeNote contactDischargeNote)
        {
            return new HttpResult<Response<ContactDischargeNote>>(_contactDischargeNoteDataProvider.AddContactDischargeNote(contactDischargeNote), Request);
        }

        /// <summary>
        /// Updates the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateContactDischargeNote(ContactDischargeNote contactDischargeNote)
        {
            return new HttpResult<Response<ContactDischargeNote>>(_contactDischargeNoteDataProvider.UpdateContactDischargeNote(contactDischargeNote), Request);
        }


        /// <summary>
        /// delete the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        public IHttpActionResult DeleteContactDischargeNote(long contactDischargeNoteID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactDischargeNote>>(_contactDischargeNoteDataProvider.DeleteContactDischargeNote(contactDischargeNoteID, modifiedOn), Request);
        }
       
    }
}
