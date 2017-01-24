using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.RuleEngine.Registration;
using Moq;
using Axis.Model.Registration;
using Axis.Model.Address;
using Axis.Model.Common;
using System.Linq;
using Axis.RuleEngine.Plugins.Registration;
using Axis.RuleEngine.Service.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Helpers;
using Axis.Service;
using System.Collections.Specialized;
using Axis.RuleEngine.RecordedServices.VoidService;
using Axis.Model.RecordedServices;
namespace Axis.RuleEngine.Tests.Controllers
{
    [TestClass]
    public class VoidServiceLiveTest
    {
        #region Class Variables

        private CommunicationManager communicationManager;
        /// <summary>
        /// The void service rule engine
        /// </summary>
        //private IVoidServiceRuleEngine voidServiceRuleEngine;
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
            Assert.IsTrue(response.RowAffected > 0,"Service cannt be voided");

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
            Assert.IsNull(response,"Response is null");
            Assert.IsNull(response.DataItems, "Unable to void service");

        }

        #endregion
    }
}
