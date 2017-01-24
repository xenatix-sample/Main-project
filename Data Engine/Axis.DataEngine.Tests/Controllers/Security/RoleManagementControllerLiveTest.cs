using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Security.RoleManagement;

namespace Axis.DataEngine.Tests.Controllers.Security
{
    [TestClass]
    public class RoleManagementControllerLiveTest
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "RoleManagement/";
        private long ModuleID = 1;
        private long RoleModuleID = 1;

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void GetRoleModuleDetails_Success()
        {
            var url = baseRoute + "GetRoleModuleDetails";

            var param = new NameValueCollection();
            param.Add("ModuleID", ModuleID.ToString());

            var response = communicationManager.Get<Response<RoleModuleDetailsModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code must be 0");
        }

        [TestMethod]
        public void GetRoleModuleComponentDetails_Success()
        {
            var url = baseRoute + "GetRoleModuleComponentDetails";

            var param = new NameValueCollection();
            param.Add("RoleModuleID", RoleModuleID.ToString());

            var response = communicationManager.Get<Response<RoleModuleComponentDetailsModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code must be 0");
        }
    }
}
