using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Plugins.Registration.Translator;
using System;

namespace Axis.Plugins.Registration.Repository
{
    public class ContactDischargeNoteRepository : IContactDischargeNoteRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ContactDischargeNote/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDischargeNoteRepository" /> class.
        /// </summary>
        public ContactDischargeNoteRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDischargeNoteRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ContactDischargeNoteRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <returns></returns>
        
        public Response<ContactDischargeNoteViewModel> GetContactDischargeNote(long contactDischargeNoteID)
        {
            const string apiUrl = baseRoute + "GetContactDischargeNote";
            var param = new NameValueCollection { { "contactDischargeNoteID", contactDischargeNoteID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactDischargeNote>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Gets the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <returns></returns>
       
        public Response<ContactDischargeNoteViewModel> GetContactDischargeNotes(long contactDischargeNoteID,int noteTypeID)
        {
            const string apiUrl = baseRoute + "GetContactDischargeNotes";
            var param = new NameValueCollection { { "contactDischargeNoteID", contactDischargeNoteID.ToString(CultureInfo.InvariantCulture) }, { "noteTypeID", noteTypeID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ContactDischargeNote>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
       
        public Response<ContactDischargeNoteViewModel> AddContactDischargeNote(ContactDischargeNoteViewModel contactDischargeNote)
        {
            string apiUrl = baseRoute + "AddContactDischargeNote";
            var response = communicationManager.Post<ContactDischargeNote, Response<ContactDischargeNote>>(contactDischargeNote.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
       
        public Response<ContactDischargeNoteViewModel> UpdateContactDischargeNote(ContactDischargeNoteViewModel contactDischargeNote)
        {
            string apiUrl = baseRoute + "UpdateContactDischargeNote";
            var response = communicationManager.Put<ContactDischargeNote, Response<ContactDischargeNote>>(contactDischargeNote.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
       
        public Response<ContactDischargeNoteViewModel> DeleteContactDischargeNote(long contactDischargeNoteID, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"contactDischargeNoteID", contactDischargeNoteID.ToString()}
            };
            return communicationManager.Delete<Response<ContactDischargeNoteViewModel>>(param, apiUrl);
        }

    }
}
