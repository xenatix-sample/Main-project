using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model;
using Axis.Service;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers.CallCenter.CallCenterProgressNote
{
    [TestClass]
    public class CallCenterProgressNoteLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "CallCenterProgressNote/";


        /// <summary>
        /// The request model
        /// </summary>
        private CallCenterProgressNoteModel requestModel = null;
        private long callCenterHeaderID = 1;

        #endregion Class Variables

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
            requestModel = new CallCenterProgressNoteModel
            {
                CallCenterHeaderID = 0,
                CallCenterTypeID = 1,
                ContactID = 3,
                CallerID = 1,
                ProviderID = 1,
                ClientStatusID = 4,
                CallCenterPriorityID = 1,
                Disposition = "Testing",
                IsActive = false,
                Comments = "",
                ReasonCalled = "Testing",
                CallStartTime = DateTime.Now,
                CallStatusID = 1,
                ProgramUnitID = 1,
                CountyID = 2,
                ModifiedOn = DateTime.Now,
                ModifiedBy = 1,
                SuicideHomicideID = 3
            };
        }

        /// <summary>
        /// Get call center progress note success
        /// </summary>
        [TestMethod]
        public void GetCallCenterProgressNote_Success()
        {
            // Arrange
            var url = baseRoute + "GetCallCenterProgressNote";
            var param = new NameValueCollection();
            param.Add("callCenterHeaderID", callCenterHeaderID.ToString());

            var response = communicationManager.Get<Response<CallCenterProgressNoteModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one call center progress note must exists.");
        }

        /// <summary>
        /// Get call center progress note failed
        /// </summary>
        [TestMethod]
        public void GetCallCenterProgressNote_Failed()
        {
            // Arrange
            var url = baseRoute + "GetCallCenterProgressNote";
            var param = new NameValueCollection();
            param.Add("callCenterHeaderID", "-1");

            var response = communicationManager.Get<Response<CallCenterProgressNoteModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Call center progress note exists for invalid data.");
        }

        /// <summary>
        /// Add call center progress note success unit test.
        /// </summary>
        [TestMethod]
        public void AddCallCenterProgressNote_Success()
        {
            // Arrange
            var url = baseRoute + "AddCallCenterProgressNote";
            requestModel.CallCenterHeaderID = callCenterHeaderID;
            requestModel.ForceRollback = true;

            // Act
            var response = communicationManager.Post<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0 && response.RowAffected > 0, "call center progress note could not be created");
        }

        /// <summary>
        /// Add call center progress note failed unit test.
        /// </summary>
        [TestMethod]
        public void AddCallCenterProgressNote_Failed()
        {
            // Arrange
            var url = baseRoute + "AddCallCenterProgressNote";
            requestModel.CallCenterHeaderID = -1;
            requestModel.ForceRollback = true;

            // Act
            var response = communicationManager.Post<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "call center progress note created for invalid data");
        }

        /// <summary>
        /// update call center progress note success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateCallCenterProgressNote_Success()
        {
            // Arrange
            var url = baseRoute + "UpdateCallCenterProgressNote";
            requestModel.CallCenterHeaderID = callCenterHeaderID;
            requestModel.Comments = "update test";
            requestModel.ForceRollback = true;

            // Act
            var response = communicationManager.Put<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0 && response.RowAffected > 0, "call center progress note could not be updated.");
        }

        /// <summary>
        /// update call center progress note Failed unit test.
        /// </summary>
        [TestMethod]
        public void UpdateCallCenterProgressNote_Failed()
        {
            // Arrange
            var url = baseRoute + "UpdateCallCenterProgressNote";
            requestModel.CallCenterHeaderID = -1;
            requestModel.Comments = "update test";
            requestModel.ForceRollback = true;
            // Act
            var response = communicationManager.Put<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.RowAffected == 0, "call center progress note updated for invalid data.");
        }
    }
}
