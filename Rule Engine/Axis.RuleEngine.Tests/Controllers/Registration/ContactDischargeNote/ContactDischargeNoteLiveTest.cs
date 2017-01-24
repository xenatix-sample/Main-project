using System;
using System.Configuration;
using Axis.Service;
using ContactDischargeNoteModel = Axis.Model.Registration.ContactDischargeNote;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Common;

namespace Axis.RuleEngine.Tests.Controllers.Registration.ContactDischargeNote
{
    [TestClass]
    public class ContactDischargeNoteLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ContactDischargeNote/";

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactID = 1;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// Adds the contact discharge note_ success.
        /// </summary>
        [TestMethod]
        public void AddContactDischargeNote_Success()
        {
            //Arrange
            var url = baseRoute + "AddContactDischargeNote";
            var addcontactDischargeNote = new ContactDischargeNoteModel
            {
                ContactDischargeNoteID = 0,
                ContactID = contactID,
                ContactAdmissionID = null,
                DischargeReasonID = 1,
                NoteTypeID = 5,
                DischargeDate = DateTime.Now,
                NoteText = "Pass Test case for adding new Note",
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Post<ContactDischargeNoteModel, Response<ContactDischargeNoteModel>>(addcontactDischargeNote, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode == 0, "Contact discharge note could not be created.");
        }

        /// <summary>
        /// Adds the contact discharge note_failure.
        /// </summary>
        [TestMethod]
        public void AddContactDischargeNote_Failure()
        {
            //Arrange
            var url = baseRoute + "AddContactDischargeNote";
            var addcontactDischargeNote = new ContactDischargeNoteModel
            {
                ContactDischargeNoteID = 0,
                ContactID = 0,
                ContactAdmissionID = null,
                DischargeReasonID = 1,
                NoteTypeID = 5,
                DischargeDate = DateTime.Now,
                NoteText = "Pass Test case for adding new Note",
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Post<ContactDischargeNoteModel, Response<ContactDischargeNoteModel>>(addcontactDischargeNote, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode != 0, "Contact Discharge Note created with invalid data.");
        }
    }
}
