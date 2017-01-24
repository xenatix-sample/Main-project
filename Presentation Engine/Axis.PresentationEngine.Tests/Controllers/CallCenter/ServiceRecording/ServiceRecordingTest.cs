using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.CallCenter.ApiControllers;
using Axis.Plugins.CallCenter.Repository.ServiceRecording;
using System.Configuration;
using Axis.Plugins.CallCenter.Models;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Tests.Controllers.CallCenter.ServiceRecording
{
    [TestClass]
    public class ServiceRecordingTest
    {
        /// <summary>
        /// The caller header identifier
        /// </summary>
        private long callCenterHeaderID = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private ServiceRecordingController controller = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new ServiceRecordingController(new ServiceRecordingRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Get Service Recording Success unit test.
        /// </summary>
        [TestMethod]
        public void GetServiceRecording_Success()
        {
            // Act
            var response = controller.GetServiceRecording(callCenterHeaderID, 1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one service recording must exist.");
        }

        /// <summary>
        /// Get Service Record failed unit test.
        /// </summary>
        [TestMethod]
        public void GetServiceRecording_Failed()
        {
            // Act
            var response = controller.GetServiceRecording(-5, 1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral Client Information should not exist for this test case.");
        }

        /// <summary>
        /// Add Service Recording success unit test.
        /// </summary>
        [TestMethod]
        public void AddServiceRecording_Success()
        {
            // Arrange
            var serviceRecording = new ServiceRecordingViewModel
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
                ConversionDateTime = DateTime.Now,
                UserID = 1,
                EndDate = DateTime.Now
            };

            // Act
            var response = controller.AddServiceRecording(serviceRecording);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code should be 0");
        }

        /// <summary>
        /// Add Service Recording success unit test.
        /// </summary>
        [TestMethod]
        public void AddServiceRecording_Failed()
        {
            // Arrange
            var serviceRecording = new ServiceRecordingViewModel
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
                ConversionDateTime = DateTime.Now,
                UserID = 1,
                EndDate = DateTime.Now
            };

            // Act
            var response = controller.AddServiceRecording(serviceRecording);

            // Assert
            var rowAffected = response.RowAffected;

            // Assert
            Assert.IsTrue(rowAffected > 0);
        }

        /// <summary>
        /// Update Service Recording success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateServiceRecording_Success()
        {
            // Arrange
            var serviceRecording = new ServiceRecordingViewModel
            {
                CallCenterHeaderID = callCenterHeaderID,
                ServiceRecordingID = 1,
                ServiceRecordingSourceID = 2,
                ServiceItemID = 2,
                AttendanceStatusID = 1,
                DeliveryMethodID = 1,
                ServiceStatusID = 1,
                ServiceLocationID = 1,
                RecipientCodeID = 1,
                NumberOfRecipients = 100,
                ConversionStatusID = 1,
                ConversionDateTime = DateTime.Now,
                UserID = 1,
                EndDate = DateTime.Now
            };

            // Act
            var response = controller.UpdateServiceRecording(serviceRecording);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code should be 0");
        }

        /// <summary>
        /// Update Service Recording success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateServiceRecording_Failed()
        {
            // Arrange
            var serviceRecording = new ServiceRecordingViewModel
            {
                CallCenterHeaderID = callCenterHeaderID,
                ServiceRecordingID = -1,
                ServiceRecordingSourceID = 2,
                ServiceItemID = 2,
                AttendanceStatusID = 1,
                DeliveryMethodID = 1,
                ServiceStatusID = 1,
                ServiceLocationID = 1,
                RecipientCodeID = 1,
                NumberOfRecipients = 100,
                ConversionStatusID = 1,
                ConversionDateTime = DateTime.Now,
                UserID = 1,
                EndDate = DateTime.Now
            };

            // Act
            var response = controller.UpdateServiceRecording(serviceRecording);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0);
        }
    }
}
