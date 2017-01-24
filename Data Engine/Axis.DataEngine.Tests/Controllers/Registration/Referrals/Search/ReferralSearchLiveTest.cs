using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Registration.Referrals;
using Axis.Model.Common;
using System.Globalization;

namespace Axis.DataEngine.Tests.Controllers.Registration.Referrals.Search
{
    [TestClass]
    public class ReferralSearchLiveTest
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private CommunicationManager _communicationManager;
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ReferralSearch/";

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
        public void GetReferrals_Success()
        {
            //Arrange
            string searchStr = "";
            const string url = baseRoute + "GetReferrals";
            var param = new NameValueCollection { { "searchStr", searchStr.ToString(CultureInfo.InvariantCulture) }, { "searchType", "1" }, { "userID", "1" } };

            //Act
            var response = _communicationManager.Get<Response<ReferralSearchModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Referral must exists.");
        }

        /// <summary>
        /// Gets the referrals_ failure.
        /// </summary>
        [TestMethod]
        public void GetReferrals_Failure()
        {
            //Arrange
            const string url = baseRoute + "GetReferrals";
            var param = new NameValueCollection { { "searchStr", "Invalid data search" }, { "searchType", "-1" }, { "userID", "0" } };

            //Act
            var response = _communicationManager.Get<Response<ReferralSearchModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Data exists for invalid data.");
        }

        /// <summary>
        /// Deletes the Referral_ success.
        /// </summary>
        [TestMethod]
        public void DeleteReferral_Success()
        {
            // Arrange
            const string apiUrl = baseRoute + "DeleteReferral";
            var requestId = new NameValueCollection { 
                                    { "id", "2" },
                                    { "reasonForDelete", "Data delete test case success".ToString(CultureInfo.InvariantCulture) } };

            // Act
            var response = _communicationManager.Delete<Response<ReferralSearchModel>>(requestId, apiUrl);


            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 3, "Referral could not be deleted.");
        }

        /// <summary>
        /// Deletes the collateral_ failure.
        /// </summary>
        [TestMethod]
        public void DeleteReferral_Failure()
        {
            //Arrange
            const string apiUrl = baseRoute + "DeleteReferral";
            var requestId = new NameValueCollection { 
                                    { "id", "9999" },
                                    { "reasonForDelete", "Data delete test case success".ToString(CultureInfo.InvariantCulture) } };

            // Act
            var response = _communicationManager.Delete<Response<ReferralSearchModel>>(requestId, apiUrl);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected <= 3, "Referral deleted for invalid data.");
        }
    }
}
