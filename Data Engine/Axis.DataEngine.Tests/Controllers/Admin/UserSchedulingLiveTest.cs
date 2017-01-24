using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Collections.Specialized;
using Axis.Model.Common;
using System.Configuration;
using Axis.Model.Admin;
using Axis.Model.Admin.UserScheduling;
using System.Collections.Generic;

namespace Axis.DataEngine.Tests.Controllers.Admin
{
    [TestClass]
    public class UserSchedulingLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "UserScheduling/";


        /// <summary>
        /// The request model
        /// </summary>
        //private UserSchedulingModel requestModel = null;
        private long userID = 1;

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
        public void GetUserFacilities_Success()
        {
            var url = baseRoute + "GetUserFacilities";

            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());

            var response = communicationManager.Get<Response<UserSchedulingModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code must be 0");
        }

        [TestMethod]
        public void GetUserFacilitySchedule_Success()
        {
            var url = baseRoute + "GetUserFacilitySchedule";

            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());
            param.Add("facilityID", 1.ToString());

            var response = communicationManager.Get<Response<UserSchedulingModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code must be 0");
        }

        [TestMethod]
        public void GetUserFacilitySchedule_Failed()
        {
            var url = baseRoute + "GetUserFacilitySchedule";

            var param = new NameValueCollection();
            param.Add("userID", "-1");
            param.Add("facilityID", 1.ToString());

            var response = communicationManager.Get<Response<UserSchedulingModel>>(param, url);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Facility Schedule must exists.");
        }

        [TestMethod]
        public void SaveUserFacilitySchedule_Success()
        {
            var url = baseRoute + "SaveUserFacilitySchedule";

            var scheduleMonday = new UserFacilitySchedulingModel{
                 FacilityID = 3,
                 DayOfWeekID = 1,
                 ScheduleTypeID = 4,
                 UserFacilityTimeSchedule = new  List<UserFacilityTimeSchedulingModel>{
                     new  UserFacilityTimeSchedulingModel{ResourceAvailabilityID=0, AvailabilityStartTime="0800", AvailabilityEndTime="1200"},
                     new  UserFacilityTimeSchedulingModel{ResourceAvailabilityID=0, AvailabilityStartTime="1300", AvailabilityEndTime="1200"}
                 }
            };

             var scheduleTuesday = new UserFacilitySchedulingModel{
                 FacilityID = 3,
                 DayOfWeekID = 2,
                 ScheduleTypeID = 4,
                 UserFacilityTimeSchedule = new  List<UserFacilityTimeSchedulingModel>{
                     new  UserFacilityTimeSchedulingModel{ResourceAvailabilityID=0, AvailabilityStartTime="0800", AvailabilityEndTime="1200"},
                     new  UserFacilityTimeSchedulingModel{ResourceAvailabilityID=0, AvailabilityStartTime="1300", AvailabilityEndTime="1200"}
                 }
            };

             var userSchedulingModel = new UserSchedulingModel
             {
                 FacilityID = 3,
                 ResourceID = 1,
                 UserFacilitySchedule = new List<UserFacilitySchedulingModel> { scheduleMonday, scheduleTuesday }
             };

             var response = communicationManager.Post<UserSchedulingModel, Response<UserSchedulingModel>>(userSchedulingModel, url);

             Assert.IsTrue(response != null, "Response can't be null");
             Assert.IsTrue(response.ResultCode == 0, "Result Code must be 0");
        }
    }
}
