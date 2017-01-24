using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Registration.ApiControllers.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Disposition;
using System.Configuration;
using System.Web.Mvc;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Model.Common;

namespace Axis.PresentationEngine.Tests.Controllers.Registration.Referrals.Disposition
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ReferralDispositionTest
    {
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "disposition/";

        /// <summary>
        /// The referral header identifier
        /// </summary>
        private long referralHeaderID = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private ReferralDispositionController controller = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new ReferralDispositionController(new ReferralDispositionRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Get Referral Disposition detail's success unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralDispositionDetail_Success()
        {
            // Act
            var response = controller.GetReferralDispositionDetail(referralHeaderID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Referral Disposition Detail must exist.");
        }

        /// <summary>
        /// Get Referral Disposition Details's failed unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralDispositionDetail_Failed()
        {
            // Act
            var response = controller.GetReferralDispositionDetail(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral disposition detail should not exist for this test case.");
        }
        /// <summary>
        /// Add Referral disposition detail's success unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralDispositionDetail_Success()
        {
            // Arrange
            var referralDispositionDetail = new ReferralDispositionViewModel
            {
                ReferralHeaderID = 2,
                ReferralDispositionID=2,
                ReferralDispositionOutcomeID=1,
                UserID=1,
                DispositionDate = DateTime.UtcNow,
                AdditionalNotes="Additional Note Test",
                ReasonforDenial="Reason for denial Test",
                ForceRollback = true
            };

            // Act
            var response = controller.AddReferralDispositionDetail(referralDispositionDetail);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected > 2, "referral disposition detail could not be created.");
        }

        /// <summary>
        /// Add Referral disposition detail's failed unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralDispositionDetail_Failed()
        {
            // Arrange
            var referralDispositionDetail = new ReferralDispositionViewModel
            {
                ReferralHeaderID = -1,
                ReferralDispositionID = 2,
                ReferralDispositionOutcomeID = 1,
                UserID = 1,
                DispositionDate = DateTime.UtcNow,
                AdditionalNotes = "Additional Note Testing",
                ReasonforDenial = "Reason for denial Testing",
                ForceRollback = true
            };

            // Act
            var response = controller.AddReferralDispositionDetail(referralDispositionDetail);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected <= 2, "Invalid referral disposition Detail has been added.");
        }

        /// <summary>
        /// Update Referral disposition detail's success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateReferralDispositionDetail_Success()
        {
            // Arrange
            var referralDispositionDetail = new ReferralDispositionViewModel
            {
                ReferralDispositionDetailID=1,
                ReferralHeaderID = referralHeaderID,
                ReferralDispositionID = 2,
                ReferralDispositionOutcomeID = 1,
                UserID = 1,
                DispositionDate = DateTime.UtcNow,
                AdditionalNotes = "Additional Note update Testing",
                ReasonforDenial = "Reason for denial update Testing",
                ForceRollback = true
            };

            // Act
            var response = controller.UpdateReferralDisposition(referralDispositionDetail);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected > 2, "Referral disposition detail could not be updated.");
        }

        /// <summary>
        /// Update Referral disposition detail's failed unit test.
        /// </summary>
        [TestMethod]
        public void UpdateReferralDispositionDetail_Failed()
        {
            // Arrange
            var referralDispositionDetail = new ReferralDispositionViewModel
            {
                ReferralDispositionDetailID=1,
                ReferralHeaderID = -1,
                ReferralDispositionID = 2,
                ReferralDispositionOutcomeID = 1,
                UserID = 1,
                DispositionDate = DateTime.UtcNow,
                AdditionalNotes = "Additional Note update Testing",
                ReasonforDenial = "Reason for denial update Testing",
                ForceRollback = true
            };

            // Act
            var response = controller.UpdateReferralDisposition(referralDispositionDetail);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected <= 2, "Updated referral disposition detail with invalid data.");
        }
       
    }
}
