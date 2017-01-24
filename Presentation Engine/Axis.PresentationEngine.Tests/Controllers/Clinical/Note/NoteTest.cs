using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Clinical.ApiControllers;
using Axis.Plugins.Clinical;
using System.Configuration;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models;

namespace Axis.PresentationEngine.Tests.Controllers.Clinical.Note
{
    [TestClass]
    public class NoteTest
    {
        /// <summary>
        /// The default contact identifier
        /// </summary>
        private int defaultContactId = 1;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteNoteID = 8;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteFailureNoteID = 7;       //Any Id which does not exist in db

        /// <summary>
        /// The controller
        /// </summary>
        private NoteController controller;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new NoteController(new NoteRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Gets the Note_success.
        /// </summary>
        [TestMethod]
        public void GetNote_Success()
        {
            // Act
            var response = controller.GetNotesAsync(defaultContactId).Result;

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Note must exists.");
        }

        /// <summary>
        /// Gets the Note_failure.
        /// </summary>
        [TestMethod]
        public void GetNote_Failure()
        {
            // Act
            var response = controller.GetNotesAsync(-1).Result;

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Note exists for invalid data.");
        }

        /// <summary>
        /// Adds the Note_ success.
        /// </summary>
        [TestMethod]
        public void AddNote_Success()
        {
            // Act
            var noteModel = new NoteViewModel
            {
                NoteID = 0,
                ContactID = 1,
                NoteTypeID = 1,
                Notes = "",
                TakenBy = 1,
                TakenTime = DateTime.Now,
                NoteStatusID = 1,
                DocumentStatusID = 1,
                EncounterID = null,
                ForceRollback = true
            };

            var response = controller.AddNote(noteModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "Note could not be created.");
        }

        /// <summary>
        /// Adds the Note_ failure.
        /// </summary>
        [TestMethod]
        public void AddNote_Failure()
        {
            // Act
            var noteModel = new NoteViewModel
            {
                NoteID = 0,
                ContactID = -1,
                NoteTypeID = -1,
                Notes = "",
                TakenBy = -1,
                TakenTime = new DateTime(),
                NoteStatusID = 1,
                DocumentStatusID = 1,
                EncounterID = 1,
                ForceRollback = true
            };

            var response = controller.AddNote(noteModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Note created for invalid data.");
        }

        /// <summary>
        /// Updates the Note_ success.
        /// </summary>
        [TestMethod]
        public void UpdateNote_Success()
        {
            // Act
            var noteModel = new NoteViewModel
            {
                NoteID = 1,
                ContactID = 1,
                NoteTypeID = 1,
                Notes = "",
                TakenBy = 1,
                TakenTime = DateTime.Now,
                NoteStatusID = 1,
                DocumentStatusID = 1,
                EncounterID = null,
                ForceRollback = true
            };

            var response = controller.UpdateNote(noteModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "Note could not be updated.");
        }

        /// <summary>
        /// Updates the Note_ failure.
        /// </summary>
        [TestMethod]
        public void UpdateNote_Failure()
        {
            // Act
            var noteModel = new NoteViewModel
            {
                NoteID = -1,
                ContactID = -1,
                NoteTypeID = 1,
                Notes = "",
                TakenBy = 1,
                TakenTime = DateTime.Now,
                NoteStatusID = 1,
                DocumentStatusID = 1,
                EncounterID = -1,
                ForceRollback = true
            };

            var response = controller.UpdateNote(noteModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Note updated for invalid data.");
        }

        /// <summary>
        /// Updates the NoteDetail_ success.
        /// </summary>
        [TestMethod]
        public void UpdateNoteDetail_Success()
        {
            // Act
            var noteModel = new NoteDetailsViewModel
            {
                NoteID = 1,
                Notes = "Note Update",
                ForceRollback = true
            };

            var response = controller.UpdateNoteDetails(noteModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Note details could not be updated.");     //Update is logged operation, default rows affected is 2
        }

        /// <summary>
        /// Updates the NoteDetails_ failure.
        /// </summary>
        [TestMethod]
        public void UpdateNoteDetails_Failure()
        {
            // Act
            var noteModel = new NoteDetailsViewModel
            {
                NoteID = -1,
                Notes = "Note Failure Update",
                ForceRollback = true
            };

            var response = controller.UpdateNoteDetails(noteModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 2, "Note details updated for invalid data.");     //Update is logged operation, default rows affected is 2
        }

        /// <summary>
        /// Deletes the Note_ success.
        /// </summary>
        [TestMethod]
        public void DeleteNote_Success()
        {
            // Act
            var response = controller.DeleteNote(defaultDeleteNoteID, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Note could not be deleted.");     //Delete is logged operation, default rows affected is 2
        }

        /// <summary>
        /// Deletes the collateral_ failure.
        /// </summary>
        [TestMethod]
        public void DeleteNote_Failure()
        {
            // Act
            var response = controller.DeleteNote(defaultDeleteFailureNoteID, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 2, "Note deleted for invalid data.");     //Delete is logged operation, default rows affected is 2
        }
    }
}
