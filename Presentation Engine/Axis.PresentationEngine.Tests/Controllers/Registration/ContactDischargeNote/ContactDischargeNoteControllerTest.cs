using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Registration.ApiControllers;
using Axis.Model.Registration;
using System.Configuration;
using MvcControllers = Axis.Plugins.Registration.Controllers;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using System.Web.Mvc;
using Axis.Model.Common;

namespace Axis.PresentationEngine.Tests.Controllers
{
    [TestClass]
    public class ContactDischargeNoteControllerTest
    {
        #region Class Variables

        /// <summary>
        /// The Contact discharge note repository
        /// </summary>
        private ContactDischargeNoteRepository _repContactDischargeController;

        #endregion

        #region Test Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _repContactDischargeController = new ContactDischargeNoteRepository();
        }

        #endregion

        #region Json Results

        #region Private Methods

        /// <summary>
        /// Adds the contact discharge note.
        /// </summary>
        /// <returns></returns>
        private Response<ContactDischargeNoteViewModel> AddContactDischargeNote(ContactDischargeNoteViewModel contactDischargeNoteViewModel)
        {
            return _repContactDischargeController.AddContactDischargeNote(contactDischargeNoteViewModel);
        }

        #endregion

        /// <summary>
        /// Adds the contact discharge note success.
        /// </summary>
        [TestMethod]
        public void AddContactDischargeNote_Success()
        {
            //Arrange
            var contactDischargeNoteViewModel = new ContactDischargeNoteViewModel
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
            var response = AddContactDischargeNote(contactDischargeNoteViewModel);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.ResultCode == 0);
        }

        /// <summary>
        /// Adds the contact discharge note failed.
        /// </summary>
        [TestMethod]
        public void AddContactDischargeNote_Failed()
        {
            //Arrange
            var contactDischargeNoteViewModel = new ContactDischargeNoteViewModel
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
            var response = AddContactDischargeNote(contactDischargeNoteViewModel);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.ResultCode != 0);
        }

        #endregion
    }
}