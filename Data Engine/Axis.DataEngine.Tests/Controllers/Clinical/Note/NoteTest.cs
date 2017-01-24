using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Common;
using Axis.Model.Clinical;
using Moq;
using System.Linq;
using Axis.DataProvider.Clinical;
using Axis.DataEngine.Plugins.Clinical;
using Axis.DataEngine.Helpers.Results;

namespace Axis.RuleEngine.Tests.Controllers.Clinical.Note
{
    /// <summary>
    /// Summary description for NoteTest
    /// </summary>
    [TestClass]
    public class NoteTest
    {
        /// <summary>
        /// The Note data provider
        /// </summary>
        private INoteDataProvider noteDataProvider;

        /// <summary>
        /// The default contact identifier
        /// </summary>
        private long defaultContactId = 1;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteContactId = 1;

        /// <summary>
        /// The note controller
        /// </summary>
        private NoteController noteController;

        /// <summary>
        /// The Note data for success
        /// </summary>
        private NoteModel noteDataForSuccess;

        /// <summary>
        /// The note response
        /// </summary>
        private Response<NoteModel> noteResponse;

        /// <summary>
        /// The Note list
        /// </summary>
        private List<NoteModel> notes;

        /// <summary>
        /// The mock
        /// </summary>
        private Mock<INoteDataProvider> mock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            InitializeMock();

            noteDataForSuccess = new NoteModel()
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
        }

        /// <summary>
        /// Initializes the mock.
        /// </summary>
        private void InitializeMock()
        {
            mock = new Mock<INoteDataProvider>();
            noteDataProvider = mock.Object;
            noteController = new NoteController(noteDataProvider);
        }

        /// <summary>
        /// Prepares the response.
        /// </summary>
        /// <param name="noteData">The note data.</param>
        private void PrepareResponse(NoteModel noteData)
        {
            notes = new List<NoteModel>();
            if (noteData.ContactID > 0)
                notes.Add(noteData);

            noteResponse = new Response<NoteModel>()
            {
                DataItems = notes
            };
        }

        /// <summary>
        /// Gets notes - success test case.
        /// </summary>
        [TestMethod]
        public void GetNotes_Success()
        {
            // Arrange
            PrepareResponse(noteDataForSuccess);

            mock.Setup(r => r.GetNotes(It.IsAny<long>()))
                .Callback((long id) => { noteResponse.DataItems = notes.Where(note => note.ContactID == id).ToList(); })
                .Returns(noteResponse);

            //Act
            var getNoteResult = noteController.GetNotes(defaultContactId);
            var response = getNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one Note must exists.");
        }

        /// <summary>
        /// Gets notes - failure test case.
        /// </summary>
        [TestMethod]
        public void GetNotes_Failure()
        {
            // Arrange
            PrepareResponse(noteDataForSuccess);

            mock.Setup(r => r.GetNotes(It.IsAny<long>()))
                .Callback((long id) => { noteResponse.DataItems = notes.Where(note => note.ContactID == id).ToList(); })
                .Returns(noteResponse);

            //Act
            var getNoteResult = noteController.GetNotes(-1);
            var response = getNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "Note exists for invalid data.");
        }

        /// <summary>
        /// Add note - success test case
        /// </summary>
        [TestMethod]
        public void AddNote_Success()
        {
            // Arrange
            //PrepareResponse(noteDataForSuccess);
            notes = new List<NoteModel>();
            noteResponse = new Response<NoteModel>()
            {
                DataItems = notes
            };

            mock.Setup(r => r.AddNote(It.IsAny<NoteModel>()))
                .Callback((NoteModel noteModel) => { if (noteModel.NoteID > 0) notes.Add(noteModel); })
                .Returns(noteResponse);

            //Act
            var addNote = new NoteModel()
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
            Assert.IsTrue(response.Value.DataItems.Count == 1, "Note could not be created.");
        }

        /// <summary>
        /// Add note - failure test case
        /// </summary>
        [TestMethod]
        public void AddNote_Failure()
        {
            // Arrange
            notes = new List<NoteModel>();
            noteResponse = new Response<NoteModel>()
            {
                DataItems = notes
            };

            mock.Setup(r => r.AddNote(It.IsAny<NoteModel>()))
                .Callback((NoteModel noteModel) => { if (noteModel.NoteID > 0) notes.Add(noteModel); })
                .Returns(noteResponse);

            //Act
            var addNote = new NoteModel()
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

            var addNoteResult = noteController.AddNote(addNote);
            var response = addNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 0, "Note created for invalid data.");
        }

        /// <summary>
        /// Update note - success test case.
        /// </summary>
        [TestMethod]
        public void UpdateNote_Success()
        {
            // Arrange
            PrepareResponse(noteDataForSuccess);

            mock.Setup(r => r.UpdateNote(It.IsAny<NoteModel>()))
                .Callback((NoteModel noteModel) => { if (noteModel.NoteID > 0) { notes.Remove(notes.Find(note => note.NoteID == noteModel.NoteID)); notes.Add(noteModel); } })
                .Returns(noteResponse);

            //Act
            var updateNote = new NoteModel()
            {
                NoteID = 1,
                ContactID = 1,
                NoteTypeID = 2,
                Notes = "Update Success test case",
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
            Assert.IsTrue(response.Value.DataItems[0].Notes == "Update Success test case", "Note could not be updated.");
        }

        /// <summary>
        /// Update note - failure test case.
        /// </summary>
        [TestMethod]
        public void UpdateNote_Failure()
        {
            // Arrange
            PrepareResponse(noteDataForSuccess);

            mock.Setup(r => r.UpdateNote(It.IsAny<NoteModel>()))
                .Callback((NoteModel noteModel) => { if (noteModel.NoteID > 0) { notes.Remove(notes.Find(note => note.NoteID == noteModel.NoteID)); notes.Add(noteModel); } })
                .Returns(noteResponse);

            //Act
            var updateNote = new NoteModel()
            {
                NoteID = -1,
                ContactID = -1,
                NoteTypeID = 1,
                Notes = "Update failure test case",
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
            Assert.IsTrue(response.Value.DataItems[0].Notes != "Update failure test case", "Note updated for invalid data.");
        }

        [TestMethod]
        public void UpdateNoteDetails_Success()
        {
            // Arrange
            var noteDetails = new List<NoteDetailsModel> { new NoteDetailsModel { NoteID = 1, Notes = "Updated Note details", ForceRollback = true } };

            var noteDetailResponse = new Response<NoteDetailsModel>()
            {
                DataItems = noteDetails
            };

            mock.Setup(r => r.UpdateNoteDetails(It.IsAny<NoteDetailsModel>()))
                .Callback((NoteDetailsModel noteModel) => { if (noteModel.NoteID > 0) { noteDetails.Remove(noteDetails.Find(note => note.NoteID == noteModel.NoteID)); noteDetails.Add(noteModel); } })
                .Returns(noteDetailResponse);

            var updateNote = new NoteDetailsModel
            {
                NoteID = 1,
                Notes = "Updated Note details Success",
                ForceRollback = true
            };

            //Act
            var updateNoteResult = noteController.UpdateNoteDetails(updateNote);
            var response = updateNoteResult as HttpResult<Response<NoteDetailsModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].Notes == "Updated Note details Success", "Note could not be updated.");
        }

        [TestMethod]
        public void UpdateNoteDetails_Failure()
        {
            //Arrange
            var noteDetails = new List<NoteDetailsModel> { new NoteDetailsModel { NoteID = 1, Notes = "Updated Note details", ForceRollback = true } };

            var noteDetailResponse = new Response<NoteDetailsModel>()
            {
                DataItems = noteDetails
            };

            mock.Setup(r => r.UpdateNoteDetails(It.IsAny<NoteDetailsModel>()))
                .Callback((NoteDetailsModel noteModel) => { if (noteModel.NoteID > 0) { noteDetails.Remove(noteDetails.Find(note => note.NoteID == noteModel.NoteID)); noteDetails.Add(noteModel); } })
                .Returns(noteDetailResponse);

            var updateNote = new NoteDetailsModel
            {
                NoteID = -1,
                Notes = "Updated Note details Failure",
                ForceRollback = true
            };

            //Act
            var updateNoteResult = noteController.UpdateNoteDetails(updateNote);
            var response = updateNoteResult as HttpResult<Response<NoteDetailsModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].Notes == "Updated Note details", "Note updated for invalid data.");
        }

        /// <summary>
        /// Delete note - success test case.
        /// </summary>
        [TestMethod]
        public void DeleteNote_Success()
        {
            // Arrange
            notes = new List<NoteModel>();
            notes.Add(noteDataForSuccess);

            mock.Setup(r => r.DeleteNote(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => notes.Remove(notes.Find(note => note.NoteID == id)))
                .Returns(noteResponse);

            //Act
            var deleteNoteResult = noteController.DeleteNote(defaultDeleteContactId, DateTime.UtcNow);
            var response = deleteNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(notes.Count() == 0, "Note could not be deleted.");
        }

        /// <summary>
        /// Delete Note - failure test case.
        /// </summary>
        [TestMethod]
        public void DeleteNote_Failure()
        {
            // Arrange
            notes = new List<NoteModel>();
            notes.Add(noteDataForSuccess);

            mock.Setup(r => r.DeleteNote(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => notes.Remove(notes.Find(note => note.NoteID == id)))
                .Returns(noteResponse);

            //Act
            var deleteNoteResult = noteController.DeleteNote(-1, DateTime.UtcNow);
            var response = deleteNoteResult as HttpResult<Response<NoteModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(notes.Count() > 0, "Note deleted for invalid record.");
        }

    }
}
