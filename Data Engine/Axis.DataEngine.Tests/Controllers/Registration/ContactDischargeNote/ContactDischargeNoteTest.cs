using System;
using System.Collections.Generic;
using Moq;
using ContactDischargeNoteModel = Axis.Model.Registration.ContactDischargeNote;
using Axis.DataEngine.Plugins.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.DataEngine.Helpers.Results;

namespace Axis.DataEngine.Tests.Controllers.Registration.ContactDischargeNote
{
    [TestClass]
    public class ContactDischargeNoteTest
    {
        /// <summary>
        /// The contact dicharge note data provider
        /// </summary>
        private IContactDischargeNoteDataProvider contactDischargeNoteDataProvider;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            
        }

        /// <summary>
        /// Adds the contact discharge note_success.
        /// </summary>
        [TestMethod]
        public void AddContactDischargeNote_Success()
        {
            Mock<IContactDischargeNoteDataProvider> mock = new Mock<IContactDischargeNoteDataProvider>();
            contactDischargeNoteDataProvider = mock.Object;
            ContactDischargeNoteController contactDischargeNoteController = new ContactDischargeNoteController(contactDischargeNoteDataProvider);

            var addContactDischargeNote = new ContactDischargeNoteModel()
            {
                ContactDischargeNoteID = 0,
                ContactID = 1,
                ContactAdmissionID = null,
                DischargeReasonID = 1,
                NoteTypeID = 5,
                DischargeDate = DateTime.Now,
                NoteText = "Pass Test case for adding new Note",
                ForceRollback = true
            };

            //Act
            var addResult = contactDischargeNoteController.AddContactDischargeNote(addContactDischargeNote);
            var response = addResult as HttpResult<Response<ContactDischargeNoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode == 0, "Contact Discharge Note created.");
        }

        /// <summary>
        /// Adds the contact discharge note_failure.
        /// </summary>
        [TestMethod]
        public void AddContactDischargeNote_Failure()
        {
            Mock<IContactDischargeNoteDataProvider> mock = new Mock<IContactDischargeNoteDataProvider>();
            contactDischargeNoteDataProvider = mock.Object;
            ContactDischargeNoteController contactDischargeNoteController = new ContactDischargeNoteController(contactDischargeNoteDataProvider);

            var addContactDischargeNote = new ContactDischargeNoteModel()
            {
                ContactDischargeNoteID = 0,
                ContactID = 0,
                ContactAdmissionID = null,
                DischargeReasonID = 1,
                NoteTypeID = 5,
                DischargeDate = DateTime.Now,
                NoteText = "Fail Test case for adding new Note",
                ForceRollback = true
            };

            //Act
            var addResult = contactDischargeNoteController.AddContactDischargeNote(addContactDischargeNote);
            var response = addResult as HttpResult<Response<ContactDischargeNoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode != 0);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }

    }
}
