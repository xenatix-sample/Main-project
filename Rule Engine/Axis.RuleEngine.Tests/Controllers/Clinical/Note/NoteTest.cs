using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Common;
using Axis.RuleEngine.Helpers.Results;
using Axis.Model.Clinical;
using Axis.RuleEngine.Clinical;
using Axis.RuleEngine.Plugins.Clinical;
using Moq;
using System.Linq;

namespace Axis.RuleEngine.Tests.Controllers.Clinical.Note
{
    /// <summary>
    /// Summary description for NoteTest
    /// </summary>
    [TestClass]
    public class NoteTest
    {
        private INoteRuleEngine noteRuleEngine;

        private long _defaultContactId = 1;
        private long _defaultDeleteNoteId = 1;
        private NoteController noteController;
        List<NoteModel> notes;
        List<NoteDetailsModel> noteDetails;

        [TestInitialize]
        public void Initialize()
        {
        }

        public void Mock_Note()
        {
            var mock = new Mock<INoteRuleEngine>();
            noteRuleEngine = mock.Object;

            noteController = new NoteController(noteRuleEngine);

            notes = new List<NoteModel>();
            notes.Add(new NoteModel()
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
            });
            var noteResponse = new Response<NoteModel>()
            {
                DataItems = notes
            };

            noteDetails = new List<NoteDetailsModel>();
            noteDetails.Add(new NoteDetailsModel()
            {
                NoteID = 1,
                Notes = "",
                ForceRollback = true
            });

            var noteDetailsResponse = new Response<NoteDetailsModel>()
            {
                DataItems = noteDetails
            };

            //Response<NoteModel> noteResponse = new Response<NoteModel>();

            //Get Note
            mock.Setup(r => r.GetNotes(It.IsAny<long>()))
                .Callback((long id) => { noteResponse.DataItems = notes.Where(note => note.ContactID == id).ToList(); })
                .Returns(noteResponse);

            //Add Note
            mock.Setup(r => r.AddNote(It.IsAny<NoteModel>()))
                .Callback((NoteModel noteModel) => { if (noteModel.ContactID > 0) notes.Add(noteModel); })
                .Returns(noteResponse);

            //Update Note
            mock.Setup(r => r.UpdateNote(It.IsAny<NoteModel>()))
                .Callback((NoteModel noteModel) => { if (noteModel.NoteID > 0) { notes.Remove(notes.Find(note => note.NoteID == noteModel.NoteID)); notes.Add(noteModel); } })
                .Returns(noteResponse);

            //Update NoteDetails
            mock.Setup(r => r.UpdateNoteDetails(It.IsAny<NoteDetailsModel>()))
                .Callback((NoteDetailsModel noteModel) => { if (noteModel.NoteID > 0) { noteDetails.Remove(noteDetails.Find(note => note.NoteID == noteModel.NoteID)); noteDetails.Add(noteModel); } })
                .Returns(noteDetailsResponse);

            //Delete Note
            var deleteResponse = new Response<NoteModel>();
            mock.Setup(r => r.DeleteNote(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => notes.Remove(notes.Find(note => note.NoteID == id)))
                .Returns(deleteResponse);
        }

        [TestMethod]
        public void GetNotes_Success()
        {
            //Arrange
            Mock_Note();

            //Act
            var getNoteResult = noteController.GetNotes(_defaultContactId);
            var response = getNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response Value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one Note must exist.");
        }

        [TestMethod]
        public void GetNotes_Failure()
        {
            //Act
            Mock_Note();

            var getNoteResult = noteController.GetNotes(-1);
            var response = getNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response Value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "Atleast one Note must exist.");
        }

        [TestMethod]
        public void AddNote_Success()
        {
            //Act
            Mock_Note();
            var addNote = new NoteModel
            {
                NoteID = 2,
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

            var addNoteResult = noteController.AddNote(addNote);
            var response = addNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 2, "Note could not be created.");
        }

        [TestMethod]
        public void AddNote_Failure()
        {
            //Act
            Mock_Note();

            var addNoteFailure = new NoteModel
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

            var addNoteResult = noteController.AddNote(addNoteFailure);
            var response = addNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 1, "Note created for invalid record.");
        }

        [TestMethod]
        public void UpdateNote_Success()
        {
            //Act
            Mock_Note();
            var updateNote = new NoteModel
            {
                NoteID = 1,
                ContactID = 1,
                NoteTypeID = 2,
                Notes = "",
                TakenBy = 1,
                TakenTime = DateTime.Now,
                NoteStatusID = 1,
                DocumentStatusID = 1,
                EncounterID = null,
                ForceRollback = true
            };
            var updateNoteResult = noteController.UpdateNote(updateNote);
            var response = updateNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].NoteTypeID == 2, "Note could not be updated.");
        }

        [TestMethod]
        public void UpdateNote_Failure()
        {
            //Act
            Mock_Note();
            var updateNote = new NoteModel
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
            var updateNoteResult = noteController.UpdateNote(updateNote);
            var response = updateNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].ContactID != -1, "Note updated for invalid data.");
        }

        [TestMethod]
        public void UpdateNoteDetails_Success()
        {
            //Act
            Mock_Note();
            var updateNote = new NoteDetailsModel
            {
                NoteID = 1,
                Notes = "Updated Note details",
                ForceRollback = true
            };
            var updateNoteResult = noteController.UpdateNoteDetails(updateNote);
            var response = updateNoteResult as HttpResult<Response<NoteDetailsModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].Notes == "Updated Note details", "Note could not be updated.");
        }

        [TestMethod]
        public void UpdateNoteDetails_Failure()
        {
            //Act
            Mock_Note();
            var updateNote = new NoteDetailsModel
            {
                NoteID = -1,
                Notes = "Failure Note Update",
                ForceRollback = true
            };
            var updateNoteResult = noteController.UpdateNoteDetails(updateNote);
            var response = updateNoteResult as HttpResult<Response<NoteDetailsModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].Notes != "Failure Note Update", "Note updated for invalid data.");
        }

        [TestMethod]
        public void DeleteNote_Success()
        {
            //Act
            Mock_Note();
            var deleteNoteResult = noteController.DeleteNote(_defaultDeleteNoteId, DateTime.UtcNow);
            var response = deleteNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(notes.Count() == 0, "Note could not be deleted.");
        }

        [TestMethod]
        public void DeleteNote_Failure()
        {
            //Act
            Mock_Note();
            var deleteNoteResult = noteController.DeleteNote(-1, DateTime.UtcNow);
            var response = deleteNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response can't be null");
            Assert.IsTrue(notes.Count() > 0, "Note deleted for invalid record.");
        }
    }
}
