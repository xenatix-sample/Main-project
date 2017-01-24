using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.BusinessAdmin;

namespace Axis.DataEngine.Tests.Controllers.BusinessAdmin.ClientMerge
{
    [TestClass]
    public class ClientMergeControllerLiveTest
    {
        #region Class Variables

        private CommunicationManager communicationManager;

        private const string baseRoute = "ClientMerge/";

        #endregion Class Variables

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void MergeRecords_Success()
        {
            var url = baseRoute + "MergeRecords";

            var clientMerge = new ClientMergeModel
            {
                ParentMRN = 1,
                ChildMRN = 2,
            };

            // Act
            var response = communicationManager.Post<ClientMergeModel, Response<ClientMergeModel>>(clientMerge, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code must be 0");
        }
    }
}
