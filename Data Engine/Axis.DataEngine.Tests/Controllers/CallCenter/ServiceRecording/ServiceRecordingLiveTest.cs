using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using Axis.Model.CallCenter;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;

namespace Axis.DataEngine.Tests.Controllers.CallCenter.ServiceRecording
{
    [TestClass]
    public class ServiceRecordingLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ServiceRecording/";


        /// <summary>
        /// The request model
        /// </summary>
        //private ServiceRecordingModel requestModel = null;
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
        }

        [TestMethod]
        public void GetServiceRecording_Success()
        {
            var url = baseRoute + "GetServiceRecording";

            var param = new NameValueCollection();
            param.Add("callCenterHeaderID", callCenterHeaderID.ToString());
            param.Add("ServiceRecordingSourceID", "1");
            var response = communicationManager.Get<Response<ServiceRecordingModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Service Recording must exists.");
        }

        [TestMethod]
        public void GetServiceRecording_Failed()
        {
            var url = baseRoute + "GetServiceRecording";

            var param = new NameValueCollection();
            param.Add("callCenterHeaderID", "-1");
            param.Add("ServiceRecordingSourceID", "1");
            var response = communicationManager.Get<Response<ServiceRecordingModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Service Recording must exists.");
        }

        /// <summary>
        /// Add Service Recording success unit test.
        /// </summary>
        [TestMethod]
        public void AddServiceRecording_Success()
        {
            // Arrange
            var url = baseRoute + "AddServiceRecording";
            var serviceRecording = new ServiceRecordingModel
            {
                CallCenterHeaderID = callCenterHeaderID,
                ServiceRecordingSourceID = 1,
                ServiceItemID = 1,
                AttendanceStatusID = 1,
                DeliveryMethodID = 1,
                ServiceStatusID = 1,
                ServiceLocationID = 1,
                RecipientCodeID = 1,
                NumberOfRecipients = 100,
                ConversionStatusID = 1,
                ConversionDateTime = DateTime.Now
            };


            // Act
            var response = communicationManager.Post<ServiceRecordingModel, Response<ServiceRecordingModel>>(serviceRecording, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Service Recording could not be created");
            Assert.IsTrue(response.RowAffected > 0, "Service Recording could not be created");
        }

        /// <summary>
        /// Add Service Recording success unit test.
        /// </summary>
        [TestMethod]
        public void AddServiceRecording_Failure()
        {
            // Arrange
            var url = baseRoute + "AddServiceRecording";
            var serviceRecording = new ServiceRecordingModel
            {
                CallCenterHeaderID = -1,
                ServiceRecordingSourceID = 1,
                ServiceItemID = 1,
                AttendanceStatusID = 1,
                DeliveryMethodID = 1,
                ServiceStatusID = 1,
                ServiceLocationID = 1,
                RecipientCodeID = 1,
                NumberOfRecipients = 100,
                ConversionStatusID = 1,
                ConversionDateTime = DateTime.Now
            };


            // Act
            var response = communicationManager.Post<ServiceRecordingModel, Response<ServiceRecordingModel>>(serviceRecording, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Service Recording not created");
        }
    }
}
