using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Registration.Referral;
using Axis.Model.Common;

namespace Axis.RuleEngine.Tests.Controllers.Registration.Referrals.Followup
{
    [TestClass]
    public class ReferralFollowupLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "referralFollowup/";

        /// <summary>
        /// The referral header identifier
        /// </summary>
        private long referralHeaderID = 1;

        /// <summary>
        /// The referral outcome detail identifier
        /// </summary>
        private long referralOutcomeDetailID = 1;

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
        /// GetReferralFollowups's success unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralFollowups_Success()
        {
            // Arrange
            var url = baseRoute + "GetReferralFollowups";

            var param = new NameValueCollection();
            param.Add("referralHeaderID", referralHeaderID.ToString());

            // Act
            var response = communicationManager.Get<Response<ReferralOutcomeDetailsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one referral followup must exist.");
        }

        /// <summary>
        /// GetReferralFollowups's failed unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralFollowups_Failed()
        {
            // Arrange
            var url = baseRoute + "GetReferralFollowups";

            var param = new NameValueCollection();
            param.Add("referralHeaderID", "-1");

            // Act
            var response = communicationManager.Get<Response<ReferralOutcomeDetailsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral followup should not exist for this test case.");
        }

        /// <summary>
        /// GetReferralFollowup's success unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralFollowup_Success()
        {
            // Arrange
            var url = baseRoute + "GetReferralFollowup";

            var param = new NameValueCollection();
            param.Add("referralOutcomeDetailID", referralOutcomeDetailID.ToString());

            // Act
            var response = communicationManager.Get<Response<ReferralOutcomeDetailsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one referral followup must exist.");

        }

        /// <summary>
        /// GetReferralFollowup's failed unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralFollowup_Failed()
        {
            // Arrange
            var url = baseRoute + "GetReferralFollowup";

            var param = new NameValueCollection();
            param.Add("referralOutcomeDetailID", "-1");

            // Act
            var response = communicationManager.Get<Response<ReferralOutcomeDetailsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral followup should not exist for this test case.");
        }

        /// <summary>
        /// AddReferralFollowup's success unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralFollowup_Success()
        {
            // Arrange
            var url = baseRoute + "AddReferralFollowup";

            var referral = new ReferralOutcomeDetailsModel
            {
                ReferralHeaderID = 1,
                FollowupExpected = true,
                FollowupDate = DateTime.UtcNow,
                FollowupProviderID = 4,
                FollowupOutcome = "Followup Outcome",
                IsAppointmentNotified = true,
                AppointmentNotificationMethod = "How",
                Comments = "comments",
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<ReferralOutcomeDetailsModel, Response<ReferralOutcomeDetailsModel>>(referral, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Referral followup could not be created.");
        }

        /// <summary>
        /// AddReferralFollowup's failed unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralFollowup_Failed()
        {
            // Arrange
            var url = baseRoute + "AddReferralFollowup";

            var referral = new ReferralOutcomeDetailsModel
            {
                ReferralHeaderID = -1,
                FollowupExpected = true,
                FollowupDate = DateTime.UtcNow,
                FollowupProviderID = 4,
                FollowupOutcome = null,
                IsAppointmentNotified = true,
                AppointmentNotificationMethod = null,
                Comments = null,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<ReferralOutcomeDetailsModel, Response<ReferralOutcomeDetailsModel>>(referral, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Invalid referral followup has been added.");
        }

        /// <summary>
        /// UpdateReferralFollowup's success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateReferralFollowup_Success()
        {
            // Arrange
            var url = baseRoute + "UpdateReferralFollowup";

            var referral = new ReferralOutcomeDetailsModel
            {
                ReferralOutcomeDetailID = 1,
                ReferralHeaderID = 1,
                FollowupExpected = true,
                FollowupDate = DateTime.UtcNow,
                FollowupProviderID = 5,
                FollowupOutcome = "Followup Outcome Modified",
                IsAppointmentNotified = true,
                AppointmentNotificationMethod = "How Modified",
                Comments = "Comments Modified",
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Put<ReferralOutcomeDetailsModel, Response<ReferralOutcomeDetailsModel>>(referral, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Referral followup could not be updated.");
        }

        /// <summary>
        /// UpdateReferralFollowup's failed unit test.
        /// </summary>
        [TestMethod]
        public void UpdateReferralFollowup_Failed()
        {
            // Arrange
            var url = baseRoute + "UpdateReferralFollowup";

            var referral = new ReferralOutcomeDetailsModel
            {
                ReferralOutcomeDetailID = -1,
                ReferralHeaderID = -1,
                FollowupExpected = true,
                FollowupDate = DateTime.UtcNow,
                FollowupProviderID = 5,
                FollowupOutcome = null,
                IsAppointmentNotified = true,
                AppointmentNotificationMethod = null,
                Comments = null,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Put<ReferralOutcomeDetailsModel, Response<ReferralOutcomeDetailsModel>>(referral, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "Updated referral followup with invalid data.");
        }
    }
}
