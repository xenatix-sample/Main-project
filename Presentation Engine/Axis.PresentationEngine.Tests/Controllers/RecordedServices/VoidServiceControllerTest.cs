using Axis.PresentationEngine.Areas.RecordedServices.Models;
using Axis.PresentationEngine.Areas.RecordedServices.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace Axis.PresentationEngine.Tests._controllers
{
    [TestClass]
    public class VoidServiceControllerTest
    {
        #region Class Variables

        private IVoidServiceRepository _rep;

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
            _rep = new VoidServiceRepository();
        }

        #region ActionResults

        [TestMethod]
        public void VoidRecordedService_Success()
        {
            var voidServiceViewModel = new VoidServiceViewModel
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
            var result = _rep.VoidService(voidServiceViewModel);
            Assert.IsTrue(result.RowAffected > 0);
        }

        [TestMethod]
        public void VoidRecordedService_Fail()
        {
            var voidServiceViewModel = new VoidServiceViewModel
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
            var result = _rep.VoidService(voidServiceViewModel);
            Assert.IsTrue(result.RowAffected == 0);
        }

        #endregion

        #endregion
    }
}
