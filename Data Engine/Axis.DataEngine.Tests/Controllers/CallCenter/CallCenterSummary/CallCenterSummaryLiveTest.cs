using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Common;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Collections.Specialized;
using Axis.Service;
using System.Configuration;
using Axis.Model.CallCenter;
using System.Globalization;

namespace Axis.DataEngine.Tests.Controllers.CallCenter.CallCenterSummary
{
    [TestClass]
    public class CallCenterSummaryLiveTest : BaseLiveTestController
    {
        #region Test Methods

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "CallCenterSummary/";

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public new void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }


        /// <summary>
        /// Gets call center search - success test case
        /// </summary>
        [TestMethod]
        public void GetCallCenterSummary_Success()
        {
            //Arrange
            string searchStr = "tt";
            var url = baseRoute + "GetCallCenterSummary";
            var param = new NameValueCollection();
            param.Add("SearchStr", searchStr.ToString(CultureInfo.InvariantCulture));

            //Act
            var response = communicationManager.Get<Response<CallCenterSummaryModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one call center must exists.");
        }


        /// <summary>
        /// Gets call center search - failed test case
        /// </summary>
        [TestMethod]
        public void GetCallCenterSummary_Failed()
        {
            //Arrange
            string searchStr = "invalid data";
            var url = baseRoute + "GetCallCenterSummary";
            var param = new NameValueCollection();
            param.Add("SearchStr", searchStr.ToString(CultureInfo.InvariantCulture));

            //Act
            var response = communicationManager.Get<Response<CallCenterSummaryModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "call center exists.");
        }

        #endregion
    }
}
