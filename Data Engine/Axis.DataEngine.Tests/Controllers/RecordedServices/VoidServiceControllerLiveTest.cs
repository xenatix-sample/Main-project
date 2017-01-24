using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Plugins.Registration;
using Axis.DataEngine.Service.Controllers;
using Axis.DataProvider.RecordedServices.VoidServices;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.RecordedServices;
using Axis.Model.Registration;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Axis.DataEngine.Tests.Controllers
{
    /// <summary>
    /// Represent VoidService Controller Test Class
    /// </summary>
    [TestClass]
    public class VoidServiceControllerLiveTest
    {
        /// <summary>
        /// The VoidService data provider
        /// </summary>

        #region Class Variables

        
        private CommunicationManager communicationManager;
        /// <summary>
        /// The void service data provider
        /// </summary>
        //private IVoidServiceDataProvider voidServiceDataProvider;
        private const string baseRoute = "VoidService/";
        #endregion

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }


        #region Client Search

        [TestMethod]
        public void VoidRecordedService_Success()
        {
            //Arrenge
            var url = baseRoute + "VoidRecordedService";
            var voidServiceModel = new VoidServiceModel
            {
                ServiceRecordingID = 1,
                ServiceRecordingVoidReasonID = 1,
                IncorrectOrganization = true,
                IncorrectServiceType = true,
                IncorrectServiceItem = true,
                IncorrectServiceStatus = true,
                IncorrectSupervisor = true,
                IncorrectAdditionalUser = true,
                IncorrectAttendanceStatus = true,
                IncorrectStartDate = true,
                IncorrectStartTime = true,
                IncorrectEndDate = true,
                IncorrectEndTime = true,
                IncorrectDeliveryMethod = true,
                IncorrectServiceLocation = true,
                IncorrectRecipientCode = true,
                IncorrectTrackingField = true,
                Comments = "",
                ModifiedOn = DateTime.Now
            };
            //Act
            var response = communicationManager.Post<VoidServiceModel, Response<VoidServiceModel>>(voidServiceModel, url);

            //Assert
            Assert.IsNull(response, "Unable to void service");
            Assert.IsTrue(response.RowAffected > 0, "Service cannt be voided");

        }

        [TestMethod]
        public void VoidRecordedService_Fail()
        {
            var url = baseRoute + "VoidRecordedService";
            var voidServiceModel = new VoidServiceModel
            {
                ServiceRecordingID = -1,
                ServiceRecordingVoidReasonID = 1,
                IncorrectOrganization = true,
                IncorrectServiceType = true,
                IncorrectServiceItem = true,
                IncorrectServiceStatus = true,
                IncorrectSupervisor = true,
                IncorrectAdditionalUser = true,
                IncorrectAttendanceStatus = true,
                IncorrectStartDate = true,
                IncorrectStartTime = true,
                IncorrectEndDate = true,
                IncorrectEndTime = true,
                IncorrectDeliveryMethod = true,
                IncorrectServiceLocation = true,
                IncorrectRecipientCode = true,
                IncorrectTrackingField = true,
                Comments = "",
                ModifiedOn = DateTime.Now
            };
            //Act
            var response = communicationManager.Post<VoidServiceModel, Response<VoidServiceModel>>(voidServiceModel, url);

            //Assert
            Assert.IsNull(response, "Response is null");
            Assert.IsNull(response.DataItems, "Unable to void service");

        }

        #endregion
    }
}