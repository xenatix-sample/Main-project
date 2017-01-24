using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model;
using Axis.Model.Common;
using System.Collections.Specialized;
using Axis.Service;
using System.Configuration;

namespace Axis.RuleEngine.Tests.Controllers.CallCenter
{
    [TestClass]
    public class CallerInformationLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "CallerInformation/";


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

        [TestMethod]
        public void GetCallerInformation_Success()
        {
            var url = baseRoute + "GetCallerInformation";

            var param = new NameValueCollection();
            param.Add("callCenterHeaderID", callCenterHeaderID.ToString());

            var response = communicationManager.Get<Response<CallCenterProgressNoteModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one caller information must exists.");
        }

        [TestMethod]
        public void GetCallerInformation_Failed()
        {
            var url = baseRoute + "GetCallerInformation";

            var param = new NameValueCollection();
            param.Add("callCenterHeaderID", "-1");

            var response = communicationManager.Get<Response<CallCenterProgressNoteModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "caller information doesn't exists.");
        }

        /// <summary>
        /// Add Service Recording success unit test.
        /// </summary>
        [TestMethod]
        public void AddCallerInforation_Success()
        {
            // Arrange
            var url = baseRoute + "AddCallerInformation";
            // Act
            var response = communicationManager.Post<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "caller information could not be created");
            Assert.IsTrue(response.RowAffected > 0, "caller information could not be created");
        }

        /// <summary>
        /// Add Service Recording failed unit test.
        /// </summary>
        [TestMethod]
        public void AddCallerInforation_Failed()
        {
            // Arrange
            var url = baseRoute + "AddCallerInformation";
            requestModel.CallCenterHeaderID = -1;
            // Act
            var response = communicationManager.Post<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Caller information not created");
        }

        /// <summary>
        /// update caller information success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateCallerInforation_Success()
        {
            // Arrange
            var url = baseRoute + "UpdateCallerInformation";
            requestModel.CallCenterHeaderID = callCenterHeaderID;
            requestModel.Comments = "update test";
            // Act
            var response = communicationManager.Put<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Caller information could not be updated.");
            Assert.IsTrue(response.RowAffected > 0, "Caller information could not be updated.");
        }

        /// <summary>
        /// update caller information Failed unit test.
        /// </summary>
        [TestMethod]
        public void UpdateCallerInforation_Failed()
        {
            // Arrange
            var url = baseRoute + "UpdateCallerInformation";
            requestModel.CallCenterHeaderID = -1;
            requestModel.Comments = "update test";
            // Act
            var response = communicationManager.Put<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.RowAffected == 0, "caller information updated.");
        }
    }
}
