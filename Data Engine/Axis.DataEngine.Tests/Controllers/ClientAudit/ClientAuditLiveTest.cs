using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using Axis.Model.Common.ClientAudit;
using Axis.Model.Common;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers.ClientAudit
{
    [TestClass]
    public class ClientAuditLiveTest
    {
        private CommunicationManager _communicationManager;
        const string baseRoute = "ClientAudit/";
        const long _defaultContactID = 1;

        [TestInitialize]
        public void Initialize()
        {
            _communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            _communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }
        [TestMethod]
        public void AddSreenAudit_Success()
        {
            //Arrange
            const string url = baseRoute + "AddScreenAudit";

            //Act
            var addScreenAudit = new ScreenAuditModel()
            {
                TransactionID = 1001,
                UserID = 1,
                ContactID = 1,
                DataKey = "Registration-Registration-AdditionalDemographics",
                ActionTypeID = 2
            };

            //Act
            var response = _communicationManager.Post<ScreenAuditModel, Response<ScreenAuditModel>>(addScreenAudit, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "client Audit could not be created.");
        }

        [TestMethod]
        public void AddSreenAudit_Failure()
        {
            //Arrange
            const string url = baseRoute + "AddScreenAudit";

            //Act
            var addScreenAudit = new ScreenAuditModel()
            {
                TransactionID = 1001,
                UserID = -2,
                ContactID = -2,
                DataKey = "",
                ActionTypeID = 2
            };

            //Act
            var response = _communicationManager.Post<ScreenAuditModel, Response<ScreenAuditModel>>(addScreenAudit, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "client Audit could not be created.");
        }

    }
}
