using Axis.Model.Common;
using Axis.Plugins.Scheduling.Controllers;
using Axis.Plugins.Scheduling.Models;
using Axis.Plugins.Scheduling.Repository.Resource;
using Axis.Helpers.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Tests.Controllers.Scheduling.Resource
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class ResourceLiveTest
    {
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "resource/";

        /// <summary>
        /// The facility identifier
        /// </summary>
        private int facilityId = 1;

        /// <summary>
        /// The appointment type identifier
        /// </summary>
        private int appointmentTypeID = 12;

        /// <summary>
        /// The credential identifier
        /// </summary>
        private long credentialId = 13;

        /// <summary>
        /// The resource type identifier
        /// </summary>
        private short resourceTypeId = 2;

        /// <summary>
        /// The resource identifier
        /// </summary>
        private int resourceId = 5;

        /// <summary>
        /// The controller
        /// </summary>
        private ResourcesController controller = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            controller = new ResourcesController(new ResourceRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Success test case for GetRooms
        /// </summary>
        [TestMethod]
        public void GetRooms_Success()
        {
            // Act
            var response = controller.GetRooms(facilityId);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one room must exists.");
        }

        /// <summary>
        /// Faliure test case for GetRooms
        /// </summary>
        [TestMethod]
        public void GetRooms_Failed()
        {
            // Act
            var response = controller.GetRooms(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Room should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetCredentialByAppointmentType
        /// </summary>
        [TestMethod]
        public void GetCredentialByAppointmentType_Success()
        {
            // Act
            var response = controller.GetCredentialByAppointmentType(appointmentTypeID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one credential must exists.");
        }

        /// <summary>
        /// Faliure test case for GetCredentialByAppointmentType
        /// </summary>
        [TestMethod]
        public void GetCredentialByAppointmentType_Failed()
        {
            // Act
            var response = controller.GetCredentialByAppointmentType(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Credential should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetProviderByCredential
        /// </summary>
        [TestMethod]
        public void GetProviderByCredential_Success()
        {
            // Act
            var response = controller.GetProviderByCredential(credentialId);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one provider must exists.");
        }

        /// <summary>
        /// Faliure test case for GetProviderByCredential
        /// </summary>
        [TestMethod]
        public void GetProviderByCredential_Failed()
        {
            // Act
            var response = controller.GetProviderByCredential(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Provider should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetResources
        /// </summary>
        [TestMethod]
        public void GetResources_Success()
        {
            // Act
            var response = controller.GetResources(resourceTypeId, facilityId);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one resource must exists.");
        }

        /// <summary>
        /// Faliure test case for GetResources
        /// </summary>
        [TestMethod]
        public void GetResources_Failed()
        {
            // Act
            var response = controller.GetResources(-1, -1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Resource should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetResourceDetails
        /// </summary>
        [TestMethod]
        public void GetResourceDetails_Success()
        {
            // Act
            var response = controller.GetResourceDetails(resourceId, resourceTypeId);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one resource must exists.");
        }

        /// <summary>
        /// Faliure test case for GetResourceDetails
        /// </summary>
        [TestMethod]
        public void GetResourceDetails_Failed()
        {
            // Act
            var response = controller.GetResourceDetails(-1, -1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems[0].ResourceAvailabilities.Count == 0, "Resource should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetResourceAvailability
        /// </summary>
        [TestMethod]
        public void GetResourceAvailability_Success()
        {
            // Act
            var response = controller.GetResourceAvailability(resourceId, resourceTypeId);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one resource availability must exists.");
        }

        /// <summary>
        /// Faliure test case for GetResourceAvailability
        /// </summary>
        [TestMethod]
        public void GetResourceAvailability_Failed()
        {
            // Act
            var response = controller.GetResourceAvailability(-1, -1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Resource availability should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetResourceOverrides
        /// </summary>
        [TestMethod]
        public void GetResourceOverrides_Success()
        {
            // Act
            var response = controller.GetResourceOverrides(resourceId, resourceTypeId);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one resource override must exists.");
        }

        /// <summary>
        /// Faliure test case for GetResourceOverrides
        /// </summary>
        [TestMethod]
        public void GetResourceOverrides_Failed()
        {
            // Act
            var response = controller.GetResourceOverrides(-1, -1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Resource override should not exists for this test case.");
        }

        [TestMethod]
        public void AddResourceAvailability_Success()
        {
            // TODO: Fake engine?
            //AxisEngine engine = new AxisEngine();
            //EngineContext.Replace(engine);
            var result = controller.AddResourceAvailability(new ResourceAvailabilityViewModel() { 
                 AvailabilityStartTime = "0800",
                 AvailabilityEndTime = "1600",
                 Days = "Monday",
                 FacilityID = 1,
                 ResourceID = 1,
                 ResourceTypeID = 1,
                 ScheduleType = "Other",
                 IsActive = false, 
                 ForceRollback = true });
            Assert.IsTrue(result != null, "Response can't be null");
            Assert.IsTrue(result.ResultMessage == "executed successfully");
            Assert.IsTrue(result.RowAffected > 0);
        }

        [TestMethod]
        public void AddResourceAvailability_Failed()
        {
            // TODO: Fake engine?
            //AxisEngine engine = new AxisEngine();
            //EngineContext.Replace(engine);
            var result = controller.AddResourceAvailability(new ResourceAvailabilityViewModel()
            {
                AvailabilityStartTime = "0800",
                AvailabilityEndTime = "1200",
                Days = "Tuesday",
                FacilityID = 1,
                ResourceID = -1,
                ResourceTypeID = 0,
                ScheduleType = "Other",
                IsActive = false,
                ForceRollback = true
            });
            Assert.IsTrue(result != null, "Response can't be null");
            Assert.IsTrue(result.ResultMessage == "executed successfully");
            Assert.IsTrue(result.RowAffected == 0);
        }

        [TestMethod]
        public void UpdateResourceAvailability_Success()
        {
            // TODO: Fake engine?
            //AxisEngine engine = new AxisEngine();
            //EngineContext.Replace(engine);

            // Create a resource availability first...
            var avail = controller.AddResourceAvailability(new ResourceAvailabilityViewModel()
            {
                AvailabilityStartTime = "0800",
                AvailabilityEndTime = "1600",
                Days = "Monday",
                FacilityID = 1,
                ResourceID = 1,
                ResourceTypeID = 1,
                ScheduleType = "Other",
                IsActive = false,
                ForceRollback = true
            });
            var result = controller.UpdateResourceAvailability(new ResourceAvailabilityViewModel()
            {
                ResourceAvailabilityID = avail.ID,
                AvailabilityStartTime = "0800",
                AvailabilityEndTime = "1100",
                Days = "Tuesday",
                FacilityID = 1,
                ResourceID = 1,
                ResourceTypeID = 1,
                ScheduleType = "Other",
                IsActive = false,
                ForceRollback = true
            });
            Assert.IsTrue(result != null, "Response can't be null");
            Assert.IsTrue(result.ResultMessage == "executed successfully");
            Assert.IsTrue(result.RowAffected == 0);
        }

        [TestMethod]
        public void UpdateResourceAvailability_Failed()
        {
            // TODO: Fake engine?
            //AxisEngine engine = new AxisEngine();
            //EngineContext.Replace(engine);

            var result = controller.UpdateResourceAvailability(new ResourceAvailabilityViewModel()
            {
                ResourceAvailabilityID = 0,
                AvailabilityStartTime = "0800",
                AvailabilityEndTime = "1100",
                Days = "Tuesday",
                FacilityID = 1,
                ResourceID = 1,
                ResourceTypeID = 1,
                ScheduleType = "Other",
                IsActive = false,
                ForceRollback = true
            });
            Assert.IsTrue(result != null, "Response can't be null");
            Assert.IsTrue(result.ResultMessage == "executed successfully");
            Assert.IsTrue(result.RowAffected > 0);
        }

        [TestMethod]
        public void UpdateRoom_Success()
        {
            var result = controller.UpdateRoom(new RoomViewModel()
            {
                FacilityID = 1,
                ForceRollback = true,
                IsActive = false,
                IsSchedulable = false,
                RoomCapacity = 40,
                RoomID = 1,
                RoomName = "test room"
            });
            Assert.IsTrue(result != null, "Response can't be null");
            Assert.IsTrue(result.ResultMessage == "executed successfully");
            Assert.IsTrue(result.RowAffected > 0);
        }

        [TestMethod]
        public void UpdateRoom_Failed()
        {
            var result = controller.UpdateRoom(new RoomViewModel()
            {
                FacilityID = 1,
                ForceRollback = true,
                IsActive = false,
                IsSchedulable = false,
                RoomCapacity = 40,
                RoomID = 0,
                RoomName = "nonexistent room"
            });
            Assert.IsTrue(result != null, "Response can't be null");
            Assert.IsTrue(result.ResultMessage == "executed successfully");
            Assert.IsTrue(result.RowAffected == 0);
        }

        [TestMethod]
        public void DeleteResourceAvailability_Success()
        {
            var result = controller.DeleteResourceAvailability(1, DateTime.UtcNow);
            Assert.IsTrue(result != null, "Response can't be null");
            Assert.IsTrue(result.ResultMessage == "executed successfully");
            Assert.IsTrue(result.RowAffected > 0);
        }

        [TestMethod]
        public void DeleteResourceAvailability_Failed()
        {
            var result = controller.DeleteResourceAvailability(0, DateTime.UtcNow);
            Assert.IsTrue(result != null, "Response can't be null");
            Assert.IsTrue(result.ResultMessage == "executed successfully");
            Assert.IsTrue(result.RowAffected == 0);
        }
    }
}