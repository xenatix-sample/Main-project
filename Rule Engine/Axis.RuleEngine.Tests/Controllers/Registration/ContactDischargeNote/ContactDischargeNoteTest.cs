using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContactDischargeNoteModel = Axis.Model.Registration.ContactDischargeNote;
using Axis.Model.Common;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Plugins.Registration;
using Axis.RuleEngine.Registration;

namespace Axis.RuleEngine.Tests.Controllers.Registration.ContactDischargeNote
{
    [TestClass]
    public class ContactDischargeNoteTest
    {
        /// <summary>
        /// The contact discharge note data provider
        /// </summary>
        private IContactDischargeNoteRuleEngine contactDischargeNoteRuleEngine;

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
            Mock<IContactDischargeNoteRuleEngine> mock = new Mock<IContactDischargeNoteRuleEngine>();
            contactDischargeNoteRuleEngine = mock.Object;
            ContactDischargeNoteController contactDischargeNoteController = new ContactDischargeNoteController(contactDischargeNoteRuleEngine);

            //Act
            var addContactDischargeNote = new ContactDischargeNoteModel
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
            Mock<IContactDischargeNoteRuleEngine> mock = new Mock<IContactDischargeNoteRuleEngine>();
            contactDischargeNoteRuleEngine = mock.Object;
            ContactDischargeNoteController contactDischargeNoteController = new ContactDischargeNoteController(contactDischargeNoteRuleEngine);

            //Act
            var addContactDischargeNote = new ContactDischargeNoteModel
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

            var addResult = contactDischargeNoteController.AddContactDischargeNote(addContactDischargeNote);
            var response = addResult as HttpResult<Response<ContactDischargeNoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.Value.ResultCode != 0);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }
    }
}
