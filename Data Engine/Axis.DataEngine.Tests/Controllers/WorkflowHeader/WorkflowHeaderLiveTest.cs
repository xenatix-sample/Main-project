using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using Axis.Model.Common.WorkflowHeader;
using Axis.Model.Common;
using System.Configuration;
using System.Collections.Specialized;

namespace Axis.DataEngine.Tests.Controllers.WorkflowHeader
{
    [TestClass]
    public class ClientAuditLiveTest
    {
        private CommunicationManager _communicationManager;
        const string baseRoute = "WorkflowHeader/";
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
            var addWorkflowHeader = new WorkflowHeaderModel()
            {
                RecordHeaderID = 1002,
                ContactID = 2,
                FirstName = "kishantest",
                LastName = "moothatest",
                WorkflowDataKey = "CrisisLine-CrisisLine-CrisisLine",
            };

            //Act
            var response = _communicationManager.Post<WorkflowHeaderModel, Response<WorkflowHeaderModel>>(addWorkflowHeader, url);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "client Audit could not be created.");
        }

        [TestMethod]
        public void GetWorkflowHeader_Success()
        {
            //Arrange
            const string url = baseRoute + "GetWorkflowHeader";

            //Act
            var param = new NameValueCollection();
            param.Add("workflowKey", "Registration-Registration-Demographics_test");
            param.Add("RecordPrimaryKeyValue", "1002");

            var response = _communicationManager.Get<Response<WorkflowHeaderModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one  WorkflowHeader must exists.");
           
        }
    }
}
