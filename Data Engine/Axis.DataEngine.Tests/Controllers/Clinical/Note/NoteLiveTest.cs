using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Model.Clinical;
using Axis.Model.Common;

namespace Axis.RuleEngine.Tests.Controllers.Clinical.Note
{
    [TestClass]
    public class NoteLiveTest
    {
        private CommunicationManager _communicationManager;
        const string baseRoute = "note/";
        const long _defaultContactID = 1;
        const long _defaultDeleteContactID = 1;
        const long _defaultFailureDeleteContactID = 1;
        const long _defaultFailureContactID = -1;

        [TestInitialize]
        public void Initialize()
        {
            _communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            _communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// The test method for GetNoteList success
        /// </summary>
        [TestMethod]
        public void GetNoteList_Success()
        {
            //Arrange
            const string url = baseRoute + "GetNotes";
            var param = new NameValueCollection { { "contactID", _defaultContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Get<Response<NoteModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Note must exists.");
        }

        /// <summary>
        /// The test method for GetNoteList failure
        /// </summary>
        [TestMethod]
        public void GetNoteList_Failure()
        {
            //Arrange
            const string url = baseRoute + "GetNotes";
            var param = new NameValueCollection { { "contactID", _defaultFailureContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Get<Response<NoteModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid data.");
        }

        /// <summary>
        /// The test method for AddNote success
        /// </summary>
        [TestMethod]
        public void AddNote_Success()
        {
            //Arrange
            const string url = baseRoute + "AddNote";
            var param = new NameValueCollection();
            var noteModel = new NoteModel
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

            //Act
            var response = _communicationManager.Post<NoteModel, Response<NoteModel>>(noteModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "Note could not be created.");
        }

        /// <summary>
        /// The test method for AddNote success
        /// </summary>
        [TestMethod]
        public void AddNote_Failure()
        {
            //Arrange
            const string url = baseRoute + "AddNote";
            var param = new NameValueCollection();
            var noteModel = new NoteModel
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

            //Act
            var response = _communicationManager.Post<NoteModel, Response<NoteModel>>(noteModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Note created with invalid data.");
        }

        /// <summary>
        /// The test method for UpdateNote success
        /// </summary>
        [TestMethod]
        public void UpdateNote_Success()
        {
            //Arrange
            const string url = baseRoute + "UpdateNote";
            var noteModel = new NoteModel
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

            //Act
            var response = _communicationManager.Put<NoteModel, Response<NoteModel>>(noteModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "Note could not be updated.");
        }

        /// <summary>
        /// The test method for UpdateNote failure
        /// </summary>
        [TestMethod]
        public void UpdateNote_Failure()
        {
            //Arrange
            const string url = baseRoute + "UpdateNote";
            var param = new NameValueCollection();
            var noteModel = new NoteModel
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

            //Act
            var response = _communicationManager.Put<NoteModel, Response<NoteModel>>(noteModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Note updated for invalid record.");
        }

        /// <summary>
        /// Updates the NoteDetail_ success.
        /// </summary>
        [TestMethod]
        public void UpdateNoteDetail_Success()
        {
            //Arrange
            var url = baseRoute + "UpdateNoteDetails";
            var param = new NameValueCollection();
            var noteModel = new NoteDetailsModel
            {
                NoteID = 1,
                Notes = "Note Update",
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Put<NoteDetailsModel, Response<NoteDetailsModel>>(noteModel, url);

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
            //Arrange
            const string url = baseRoute + "UpdateNoteDetails";
            var param = new NameValueCollection();
            var noteModel = new NoteDetailsModel
            {
                NoteID = -1,
                Notes = "Note Failure Update",
                ForceRollback = true
            };

            //Act
            var response = _communicationManager.Put<NoteDetailsModel, Response<NoteDetailsModel>>(noteModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 2, "Note details updated for invalid data.");     //Update is logged operation, default rows affected is 2
        }

        /// <summary>
        /// The test method for DeleteNote success
        /// </summary>
        [TestMethod]
        public void DeleteNote_Success()
        {
            //Arrange
            const string apiUrl = baseRoute + "DeleteNote";
            var param = new NameValueCollection { { "Id", _defaultDeleteContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Delete<Response<NoteModel>>(param, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Note could not be deleted.");     //Delete is logged operation, default rows affected is 2
        }

        /// <summary>
        /// The test method for DeleteNote success
        /// </summary>
        [TestMethod]
        public void DeleteNote_Failure()
        {
            //Arrange
            const string apiUrl = baseRoute + "DeleteNote";
            var param = new NameValueCollection { { "Id", _defaultFailureContactID.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Delete<Response<NoteModel>>(param, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 2, "Note deleted for invalid record.");     //Delete is logged operation, default rows affected is 2
        }
    }
}
