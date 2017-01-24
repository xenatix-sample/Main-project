using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.DataEngine.Tests.Controllers.Security
{
    [TestClass]
    public class FeatureSecurityControllerLiveTest
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "security/";
        private int userId = 1;
        private string moduleName = "Registration";
        private string featureName = "Demography";
        private string actionName = "Create";

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

            var param = new NameValueCollection();
            param.Add("UserID", userId.ToString());

            // Act
            var response = communicationManager.Get<Response<RoleSecurityModel>>(param, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one role must exists.");
        }

        [TestMethod]
        public void VerifyRolePermission_Success()
        {
            // Arrange
            var url = baseRoute + "verifyRolePermission";

            var param = new NameValueCollection();
            param.Add("userID", userId.ToString());
            param.Add("moduleName", moduleName ?? string.Empty);
            param.Add("featureName", featureName ?? string.Empty);
            param.Add("actionName", actionName ?? string.Empty);

            // Act
            var response = communicationManager.Get<Response<ScalarResult<bool>>>(param, url);

            // Assert
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one role must exists.");
        }
    }
}
