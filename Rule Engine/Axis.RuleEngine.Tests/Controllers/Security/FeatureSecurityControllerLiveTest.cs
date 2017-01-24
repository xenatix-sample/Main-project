using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.RuleEngine.Tests.Controllers.Security
{
    [TestClass]
    public class FeatureSecurityControllerLiveTest
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "security/";

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void GetUserRoleSecurity_Success()
        {
            // Arrange
            var url = baseRoute + "getUserRoleSecurity";

            // Act
            var response = communicationManager.Get<Response<RoleSecurityModel>>(url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one role must exists.");
        }
    }
}
