using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.PresentationEngine.Areas.Admin.ApiControllers;
using Axis.PresentationEngine.Areas.Admin.Models.UserScheduling;
using Axis.PresentationEngine.Areas.Admin.Respository;
using System.Configuration;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Tests.Controllers.Admin.UserScheduling
{
    [TestClass]
    public class UserSchedulingControllerLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The controller
        /// </summary>
        private UserSchedulingController controller = null;

        private readonly bool isMyProfile = true;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "UserScheduling/";


        /// <summary>
        /// The request model
        /// </summary>
        //private UserSchedulingViewModel requestModel = null;
        //private long userID = 1;

        #endregion Class Variables

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new UserSchedulingController(new UserSchedulingRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        [TestMethod]
        public void GetUserFacilities_Success()
        {
            // Act
            var response = controller.GetUserFacilities(1, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one User Facility must exist.");
        }

        [TestMethod]
        public void GetUserFacilities_Failure()
        {
            // Act
            var response = controller.GetUserFacilities(-1, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid user id");
        }

        [TestMethod]
        public void GetUserFacilitySchedule_Success()
        {
            // Act
            var response = controller.GetUserFacilitySchedule(1, 1, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one User Facility Schedule must exist.");
        }

        [TestMethod]
        public void GetUserFacilitySchedule_Failure()
        {
            // Act
            var response = controller.GetUserFacilitySchedule(-1, -1, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid user id");
        }

        [TestMethod]
        public void SaveUserFacilitySchedule_Success()
        {
            //Arrange
            var scheduleMonday = new UserFacilitySchedulingViewModel
            {
                FacilityID = 1,
                DayOfWeekID = 1,
                ScheduleTypeID = 4,
                UserFacilityTimeSchedule = new List<UserFacilityTimeSchedulingViewModel>{
                     new  UserFacilityTimeSchedulingViewModel{ResourceAvailabilityID=0, AvailabilityStartTime="0800", AvailabilityEndTime="1200"},
                     new  UserFacilityTimeSchedulingViewModel{ResourceAvailabilityID=0, AvailabilityStartTime="1300", AvailabilityEndTime="1200"}
                 }
            };

            var scheduleTuesday = new UserFacilitySchedulingViewModel
            {
                FacilityID = 3,
                DayOfWeekID = 2,
                ScheduleTypeID = 4,
                UserFacilityTimeSchedule = new List<UserFacilityTimeSchedulingViewModel>{
                     new  UserFacilityTimeSchedulingViewModel{ResourceAvailabilityID=0, AvailabilityStartTime="0800", AvailabilityEndTime="1200"},
                     new  UserFacilityTimeSchedulingViewModel{ResourceAvailabilityID=0, AvailabilityStartTime="1300", AvailabilityEndTime="1200"}
                 }
            };

            var userSchedulingModel = new UserSchedulingViewModel
            {
                FacilityID = 3,
                ResourceID = 1,
                UserFacilitySchedule = new List<UserFacilitySchedulingViewModel> { scheduleMonday, scheduleTuesday }
            };

            // Act
            var response = controller.SaveUserFacilitySchedule(userSchedulingModel, isMyProfile);


            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code must be 0");
        }
    }
}
