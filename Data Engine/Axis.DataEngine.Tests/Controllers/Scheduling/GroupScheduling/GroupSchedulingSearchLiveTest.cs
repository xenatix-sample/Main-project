using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Model.Common;
using Axis.Model.Scheduling;

namespace Axis.DataEngine.Tests.Controllers.Scheduling.GroupScheduling
{
    [TestClass]
    public class GroupSchedulingSearchLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "GroupSchedulingSearch/";

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void GetGroupSchedules_Success()
        {
            //Arrange
            string searchStr = "tt";
            var url = baseRoute + "GetGroupSchedules";
            var param = new NameValueCollection();
            param.Add("SearchStr", searchStr.ToString(CultureInfo.InvariantCulture));

            //Act
            var response = communicationManager.Get<Response<GroupSchedulingSearchModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Group Schedule must exists.");
        }
    }
}
