using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Plugins.Registration;
using Axis.DataEngine.Service.Controllers;
using Axis.DataProvider.RecordedServices.VoidServices;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.RecordedServices;
using Axis.Model.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axis.DataEngine.Tests.Controllers
{
    /// <summary>
    /// Represent VoidService Controller Test Class
    /// </summary>
    [TestClass]
    public class VoidServiceControllerTest
    {
        /// <summary>
        /// The VoidService data provider
        /// </summary>
        private IVoidServiceDataProvider voidServiceDataProvider;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
        }

        /// <summary>
        /// Adds the void service for success.
        /// </summary>
        [TestMethod]
        public void VoidRecordedService_Success()
        {
            Mock<IVoidServiceDataProvider> mock = new Mock<IVoidServiceDataProvider>();
            voidServiceDataProvider = mock.Object;
            VoidServiceController voidServiceController = new VoidServiceController(voidServiceDataProvider);
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
            var saveResult = voidServiceController.VoidRecordedService(voidServiceModel);
            var response = saveResult as HttpResult<Response<VoidServiceModel>>;
            var count = response.Value.DataItems.Count();

            //Assert
            Assert.IsNotNull(voidServiceModel);
            Assert.IsTrue(count > 0);
        }

        /// <summary>
        /// Adds the Contact demographic for failed.
        /// </summary>
        [TestMethod]
        public void AddContactDemographic_Failed()
        {
            Mock<IVoidServiceDataProvider> mock = new Mock<IVoidServiceDataProvider>();
            voidServiceDataProvider = mock.Object;
            VoidServiceController voidServiceController = new VoidServiceController(voidServiceDataProvider);
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
            var saveResult = voidServiceController.VoidRecordedService(voidServiceModel);
            var response = saveResult as HttpResult<Response<VoidServiceModel>>;
            var count = response.Value.DataItems.Count();

            //Assert
            Assert.IsTrue(count == 0);
        }
    }
}