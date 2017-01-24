using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration
{
    public class ContactDischargeNoteService : IContactDischargeNoteService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ContactDischargeNote/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDischargeNoteService" /> class.
        /// </summary>
        public ContactDischargeNoteService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> GetContactDischargeNote(long contactDischargeNoteID)
        {
            const string apiUrl = BaseRoute + "GetContactDischargeNote";
            var requestParams = new NameValueCollection { { "contactDischargeNoteID", contactDischargeNoteID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactDischargeNote>>(requestParams, apiUrl);
        }

        /// <summary>
        /// Gets the contact discharge notes.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> GetContactDischargeNotes(long contactDischargeNoteID,int noteTypeID)
        {
            const string apiUrl = BaseRoute + "GetContactDischargeNote";
            var requestParams = new NameValueCollection { { "contactDischargeNoteID", contactDischargeNoteID.ToString(CultureInfo.InvariantCulture) }, { "noteTypeID", noteTypeID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactDischargeNote>>(requestParams, apiUrl);
        }

        /// <summary>
        /// Adds the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> AddContactDischargeNote(ContactDischargeNote contactDischargeNote)
        {
            const string apiUrl = BaseRoute + "AddContactDischargeNote";
            return communicationManager.Post<ContactDischargeNote, Response<ContactDischargeNote>>(contactDischargeNote, apiUrl);
        }

        /// <summary>
        /// Updates the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> UpdateContactDischargeNote(ContactDischargeNote contactDischargeNote)
        {
            const string apiUrl = BaseRoute + "UpdateContactDischargeNote";
            return communicationManager.Put<ContactDischargeNote, Response<ContactDischargeNote>>(contactDischargeNote, apiUrl);
        }

        /// <summary>
        /// delete the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
                /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactDischargeNote> DeleteContactDischargeNote(long contactDischargeNoteID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteContactBenefit";
            var requestParams = new NameValueCollection
            {
                { "contactDischargeNoteID", contactDischargeNoteID.ToString(CultureInfo.InvariantCulture) },
                  { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
               
            };
            return communicationManager.Delete<Response<ContactDischargeNote>>(requestParams, apiUrl);
        }
    }
}
