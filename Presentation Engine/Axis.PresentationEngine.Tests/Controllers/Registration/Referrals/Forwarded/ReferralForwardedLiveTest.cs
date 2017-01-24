using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Registration.ApiControllers.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Forwarded;
using System.Configuration;
using System.Web.Mvc;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals;

namespace Axis.PresentationEngine.Tests.Controllers.Referrals.Forwarded
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ReferralForwardedLiveTest
    {
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "forwarded/";

        /// <summary>
        /// The referral header identifier
        /// </summary>
        private long ReferralHeaderID = 1;

        /// <summary>
        /// The referral outcome detail identifier
        /// </summary>
        private long ReferralForwardedDetailID = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private ReferralForwardedController controller = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new ReferralForwardedController(new ReferralForwardedRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// GetReferralforwarded success unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralForwardedDetails_Success()
        {
            // Act
            var response = controller.GetReferralForwardedDetails(ReferralHeaderID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one referral forwarded must exist.");
        }

        /// <summary>
        /// GetReferralForwardedDetails failed unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralForwardedDetails_Failed()
        {
            var response = controller.GetReferralForwardedDetails(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral forwarded should not exist for this test case.");
        }

        /// <summary>
        /// GetReferralForwardedDetail success unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralForwardedDetail_Success()
        {
            // Act
            var response = controller.GetReferralForwardedDetail(ReferralForwardedDetailID);

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
            // Act
            var response = controller.GetReferralForwardedDetail(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral forwarded should not exist for this test case.");
        }

        /// <summary>
        /// AddReferralForwardedDetail success unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralForwardedDetail_Success()
        {
            // Arrange
            var referral = new ReferralForwardedViewModel
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
            var response = controller.AddReferralForwardedDetail(referral);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Referral forwarded could not be created.");
        }

        /// <summary>
        /// AddReferralForwardedDetail failed unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralForwardedDetail_Failed()
        {
            // Arrange
            var referral = new ReferralForwardedViewModel
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
            var response = controller.AddReferralForwardedDetail(referral);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Invalid referral forwarded has been added.");
        }

        /// <summary>
        /// UpdateReferralForwardedDetail success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateReferralForwardedDetail_Success()
        {
            // Arrange
            var referral = new ReferralForwardedViewModel
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
            var response = controller.UpdateReferralForwardedDetail(referral);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Referral forwarded could not be updated.");
        }

        /// <summary>
        /// UpdateReferralForwardedDetail's failed unit test.
        /// </summary>
        [TestMethod]
        public void UpdateReferralForwardedDetail_Failed()
        {
            // Arrange
            var referral = new ReferralForwardedViewModel
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
            var response = controller.UpdateReferralForwardedDetail(referral);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "Updated referral forwarded with invalid data.");
        }
    }
}
