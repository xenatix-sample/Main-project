using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using System.Globalization;
using Axis.Model.CallCenter;


namespace Axis.RuleEngine.Tests.Controllers.CallCenter
{
    [TestClass]
    public class CallCenterSummaryLiveTest
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private CommunicationManager _communicationManager;
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "CallCenterSummary/";

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            _communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// Gets the referrals_ success.
        /// </summary>
        [TestMethod]
        public void GetCallCenterSummary_Success()
        {
            //Arrange
            string searchStr = "";
            const string url = baseRoute + "GetCallCenterSummary";
            var param = new NameValueCollection { { "searchStr", searchStr.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Get<Response<CallCenterSummaryModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one call center must exists.");
        }


        /// <summary>
        /// Gets the referrals_failed.
        /// </summary>
        [TestMethod]
        public void GetCallCenterSummary_Failed()
        {
            //Arrange
            string searchStr = "invalid data";
            const string url = baseRoute + "GetCallCenterSummary";
            var param = new NameValueCollection { { "searchStr", searchStr.ToString(CultureInfo.InvariantCulture) } };

            //Act
            var response = _communicationManager.Get<Response<CallCenterSummaryModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "call center must exists.");
        }
    }
}
