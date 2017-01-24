using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.RuleEngine.Tests.Controllers.Registration.Referrals.Forwarded
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ReferralForwardedLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "referralForwarded/";

        /// <summary>
        /// The referral header identifier
        /// </summary>
        private long ReferralHeaderID = 1;

        /// <summary>
        /// The referral forwarded detail identifier
        /// </summary>
        private long ReferralForwardedDetailID = 1;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// GetReferralforwarded details success unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralForwardedDetails_Success()
        {
            // Arrange
            var url = baseRoute + "GetReferralForwardedDetails";

            var param = new NameValueCollection();
            param.Add("ReferralHeaderID", ReferralHeaderID.ToString());

            // Act
            var response = communicationManager.Get<Response<ReferralForwardedModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one referral forwarded must exist.");
        }

        /// <summary>
        /// GetReferralForwarded failed unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralForwardedDetails_Failed()
        {
            // Arrange
            var url = baseRoute + "GetReferralForwardedDetails";

            var param = new NameValueCollection();
            param.Add("referralHeaderID", "-1");

            // Act
            var response = communicationManager.Get<Response<ReferralForwardedModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral forwarded should not exist for this test case.");
        }

        /// <summary>
        /// GetReferralForwarded success unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralForwardedDetail_Success()
        {
            // Arrange
            var url = baseRoute + "GetReferralForwardedDetail";

            var param = new NameValueCollection();
            param.Add("ReferralForwardedDetailID", ReferralForwardedDetailID.ToString());

            // Act
            var response = communicationManager.Get<Response<ReferralForwardedModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one referral forwarded must exist.");

        }

        /// <summary>
        /// GetReferralForwardedDetail failed unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralForwardedDetail_Failed()
        {
            // Arrange
            var url = baseRoute + "GetReferralForwardedDetail";

            var param = new NameValueCollection();
            param.Add("ReferralForwardedDetailID", "-1");

            // Act
            var response = communicationManager.Get<Response<ReferralForwardedModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral forwarded should not exist for this test case.");
        }

        /// <summary>
        /// AddreferralForwarded. success unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralForwardedDetail_Success()
        {
            // Arrange
            var url = baseRoute + "AddReferralForwardedDetail";

            var referral = new ReferralForwardedModel
            {
                ReferralHeaderID = 1,
                ReferralSentDate = DateTime.UtcNow,
                SendingReferralToID = 4,
                OrganizationID = 1,
                UserID = 23,
                Comments = "comments",
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<ReferralForwardedModel, Response<ReferralForwardedModel>>(referral, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Referral forwarded could not be created.");
        }

        /// <summary>
        /// AddReferralforwarded failed unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralForwardedDetail_Failed()
        {
            // Arrange
            var url = baseRoute + "AddReferralForwardedDetail";

            var referral = new ReferralForwardedModel
            {
                ReferralHeaderID = -1,
                ReferralSentDate = DateTime.UtcNow,
                SendingReferralToID = 4,
                OrganizationID = 1,
                UserID = 23,
                Comments = null,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<ReferralForwardedModel, Response<ReferralForwardedModel>>(referral, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Invalid referral forwarded has been added.");
        }

        /// <summary>
        /// UpdateReferralForwarded success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateReferralForwardedDetail_Success()
        {
            // Arrange
            var url = baseRoute + "UpdateReferralForwardedDetail";

            var referral = new ReferralForwardedModel
            {
                ReferralHeaderID = 1,
                ReferralSentDate = DateTime.UtcNow,
                SendingReferralToID = 5,
                OrganizationID = 1,
                UserID = 23,
                Comments = "Comments Modified",
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Put<ReferralForwardedModel, Response<ReferralForwardedModel>>(referral, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Referral forwarded could not be updated.");
        }

        /// <summary>
        /// UpdateReferralforwarded failed unit test.
        /// </summary>
        [TestMethod]
        public void UpdateReferralForwardedDetail_Failed()
        {
            // Arrange
            var url = baseRoute + "UpdateReferralForwardedDetail";

            var referral = new ReferralForwardedModel
            {
                ReferralForwardedDetailID = -1,
                ReferralHeaderID = -1,
                ReferralSentDate = DateTime.UtcNow,
                SendingReferralToID = 4,
                OrganizationID = 1,
                UserID = 23,
                Comments = null,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Put<ReferralForwardedModel, Response<ReferralForwardedModel>>(referral, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "Updated referral forwarded with invalid data.");
        }
    }
}
