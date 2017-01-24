using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Registration.ApiControllers.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Followup;
using System.Configuration;
using System.Web.Mvc;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals;

namespace Axis.PresentationEngine.Tests.Controllers.Referrals.Followup
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ReferralFollowupLiveTest
    {
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "followup/";

        /// <summary>
        /// The referral header identifier
        /// </summary>
        private long referralHeaderID = 1;

        /// <summary>
        /// The referral outcome detail identifier
        /// </summary>
        private long referralOutcomeDetailID = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private ReferralFollowupController controller = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new ReferralFollowupController(new ReferralFollowupRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// GetReferralFollowups's success unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralFollowups_Success()
        {
            // Act
            var response = controller.GetReferralFollowups(referralHeaderID);

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
            var response = controller.GetReferralFollowups(-1);

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
            // Act
            var response = controller.GetReferralFollowup(referralOutcomeDetailID);

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
            // Act
            var response = controller.GetReferralFollowup(-1);

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
            var referral = new ReferralOutcomeDetailsViewModel
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
            var response = controller.AddReferralFollowup(referral);

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
            var referral = new ReferralOutcomeDetailsViewModel
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
            var response = controller.AddReferralFollowup(referral);

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
            var referral = new ReferralOutcomeDetailsViewModel
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
            var response = controller.UpdateReferralFollowup(referral);

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
            var referral = new ReferralOutcomeDetailsViewModel
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
            var response = controller.UpdateReferralFollowup(referral);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "Updated referral followup with invalid data.");
        }
    }
}
